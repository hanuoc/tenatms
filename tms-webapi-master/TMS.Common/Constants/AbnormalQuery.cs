using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMS.Common.Constants
{
    public class AbnormalQuery
    {
        public const string CreateStoreAbnormalCaseQuery = @"    CREATE PROCEDURE [dbo].[USP_Abnormal] (
	            @StartDate DATETIME
	            ,@EndDate DATETIME
	            )
            AS
            BEGIN
	            DECLARE @FingerTimeSheets TABLE (
		            ID INT
		            ,UserNo INT
		            ,DayOfCheck DATETIME
		            ,CheckIn NVARCHAR(50)
		            ,Checkout NVARCHAR(50)
		            ,Late NVARCHAR(50)
		            ,LeaveEarly NVARCHAR(50)
		            ,OTCheckIn NVARCHAR(50)
		            ,OTcheckOut NVARCHAR(50)
		            ,Absent NVARCHAR(50)
		            ,NumOfWorking NVARCHAR(50)
		            ,MinusAlow NVARCHAR(50)
		            )

	            INSERT INTO @FingerTimeSheets
	            SELECT *
	            FROM FingerTimeSheets
	            WHERE DayOfCheck >= @StartDate
		            AND DayOfCheck < DATEADD(DAY, 1, @EndDate)

	            DECLARE @MyTempTable TABLE (
		            ID INT
		            ,UserNo INT
		            ,DayOfCheck DATETIME
		            ,CheckIn NVARCHAR(50)
		            ,Checkout NVARCHAR(50)
		            ,Late NVARCHAR(50)
		            ,LeaveEarly NVARCHAR(50)
		            ,OTCheckIn NVARCHAR(50)
		            ,OTcheckOut NVARCHAR(50)
		            ,Absent NVARCHAR(50)
		            ,NumOfWorking NVARCHAR(50)
		            ,MinusAlow NVARCHAR(50)
		            )

	            INSERT INTO @MyTempTable
	            SELECT *
	            FROM @FingerTimeSheets
	            WHERE ID NOT IN (
			            SELECT DISTINCT timesheet.ID
			            FROM @FingerTimeSheets AS timesheet
			            INNER JOIN dbo.FingerMachineUsers AS FingerUser ON timesheet.UserNo = FingerUser.ID
			            INNER JOIN dbo.Requests AS request ON request.UserId = FingerUser.UserId
			            WHERE timesheet.UserNo IN (
					            SELECT ID
					            FROM dbo.FingerMachineUsers
					            )
				            AND (
					            timesheet.DayOfCheck BETWEEN request.StartDate
						            AND request.EndDate
					            )
				            AND (
					            request.RequestStatusId = 4
					            AND request.EntitleDayId IS NOT NULL
					            AND (
						            request.RequestTypeId = 1
						            OR request.RequestTypeId = 2
						            OR request.RequestTypeId = 3
						            )
					            )
			
			            UNION
			
			            SELECT DISTINCT timesheet.ID
			            FROM @FingerTimeSheets AS timesheet
			            INNER JOIN dbo.FingerMachineUsers AS FingerUser ON timesheet.UserNo = FingerUser.ID
			            INNER JOIN dbo.Requests AS request ON request.UserId = FingerUser.UserId
			            WHERE timesheet.UserNo IN (
					            SELECT ID
					            FROM dbo.FingerMachineUsers
					            )
				            AND timesheet.DayOfCheck BETWEEN request.StartDate
					            AND request.EndDate
				            AND (
					            request.RequestStatusId = 4
					            AND request.EntitleDayId IS NULL
					            AND (
						            request.RequestTypeId = 5
						            OR request.RequestTypeId = 4
						            )
					            )
			            )

	            DECLARE @MyTempTable1 TABLE (
		            ID INT
		            ,UserNo INT
		            ,DayOfCheck DATETIME
		            ,CheckIn NVARCHAR(50)
		            ,Checkout NVARCHAR(50)
		            ,Late NVARCHAR(50)
		            ,LeaveEarly NVARCHAR(50)
		            ,OTCheckIn NVARCHAR(50)
		            ,OTcheckOut NVARCHAR(50)
		            ,Absent NVARCHAR(50)
		            ,NumOfWorking NVARCHAR(50)
		            ,MinusAlow NVARCHAR(50)
		            )

	            INSERT INTO @MyTempTable1
	            SELECT *
	            FROM @FingerTimeSheets

	            DECLARE @MyTempTable2 TABLE (
		            ID INT
		            ,UserNo INT
		            ,DayOfCheck DATETIME
		            ,CheckIn NVARCHAR(50)
		            ,Checkout NVARCHAR(50)
		            ,Late NVARCHAR(50)
		            ,LeaveEarly NVARCHAR(50)
		            ,OTCheckIn NVARCHAR(50)
		            ,OTcheckOut NVARCHAR(50)
		            ,Absent NVARCHAR(50)
		            ,NumOfWorking NVARCHAR(50)
		            ,MinusAlow NVARCHAR(50)
		            )

	            INSERT INTO @MyTempTable2
	            SELECT *
	            FROM @FingerTimeSheets
	            WHERE Absent IS NULL
		            AND Late IS NULL
		            AND LeaveEarly IS NULL

	            DECLARE @OTUser TABLE (
		            UserId NVARCHAR(50)
		            ,OTDate DATETIME
		            ,Title NVARCHAR(256)
		            )

	            INSERT INTO @OTUser
	            SELECT otuser.UserID
		            ,otrequets.OTDate
		            ,otrequets.Title
	            FROM dbo.OTRequests AS otrequets
	            INNER JOIN dbo.OTRequestUser AS otuser ON otrequets.ID = otuser.OTRequestID
	            WHERE otrequets.StatusRequestID = 4

	            DECLARE @MyTempTable5 TABLE (
		            ID INT
		            ,UserNo INT
		            ,DayOfCheck DATETIME
		            ,CheckIn NVARCHAR(50)
		            ,Checkout NVARCHAR(50)
		            ,Late NVARCHAR(50)
		            ,LeaveEarly NVARCHAR(50)
		            ,OTCheckIn NVARCHAR(50)
		            ,OTcheckOut NVARCHAR(50)
		            ,Absent NVARCHAR(50)
		            ,NumOfWorking NVARCHAR(50)
		            ,MinusAlow NVARCHAR(50)
		            )

	            INSERT INTO @MyTempTable5
	            SELECT *
	            FROM @FingerTimeSheets
	            WHERE ID IN (
			            SELECT DISTINCT timesheet.ID
			            FROM @FingerTimeSheets AS timesheet
			            INNER JOIN dbo.FingerMachineUsers AS FingerUser ON timesheet.UserNo = FingerUser.ID
			            INNER JOIN dbo.Requests AS request ON request.UserId = FingerUser.UserId
			            WHERE timesheet.UserNo IN (
					            SELECT ID
					            FROM dbo.FingerMachineUsers
					            )
				            AND (
					            timesheet.DayOfCheck BETWEEN request.StartDate
						            AND request.EndDate
					            )
				            AND (
					            request.RequestStatusId = 4
					            AND request.EntitleDayId IS NOT NULL
					            AND (
						            request.RequestTypeId = 1
						            OR request.RequestTypeId = 2
						            OR request.RequestTypeId = 3
						            )
					            )
			            GROUP BY timesheet.ID
			            HAVING COUNT(*) = 1
			
			            UNION
			
			            SELECT DISTINCT timesheet.ID
			            FROM @FingerTimeSheets AS timesheet
			            INNER JOIN dbo.FingerMachineUsers AS FingerUser ON timesheet.UserNo = FingerUser.ID
			            INNER JOIN dbo.Requests AS request ON request.UserId = FingerUser.UserId
			            WHERE timesheet.UserNo IN (
					            SELECT ID
					            FROM dbo.FingerMachineUsers
					            )
				            AND (
					            timesheet.DayOfCheck BETWEEN request.StartDate
						            AND request.EndDate
					            )
				            AND (
					            request.RequestStatusId = 4
					            AND request.EntitleDayId IS NULL
					            AND (
						            request.RequestTypeId = 5
						            OR request.RequestTypeId = 4
						            )
					            )
			            GROUP BY timesheet.ID
			            HAVING COUNT(*) = 1
			            )

	            DECLARE @MyTempTable6 TABLE (
		            ID INT
		            ,UserNo INT
		            ,DayOfCheck DATETIME
		            ,CheckIn NVARCHAR(50)
		            ,Checkout NVARCHAR(50)
		            ,Late NVARCHAR(50)
		            ,LeaveEarly NVARCHAR(50)
		            ,OTCheckIn NVARCHAR(50)
		            ,OTcheckOut NVARCHAR(50)
		            ,Absent NVARCHAR(50)
		            ,NumOfWorking NVARCHAR(50)
		            ,MinusAlow NVARCHAR(50)
		            )

	            INSERT INTO @MyTempTable6
	            SELECT *
	            FROM @FingerTimeSheets
	            WHERE ID IN (
			            SELECT DISTINCT timesheet.ID
			            FROM @FingerTimeSheets AS timesheet
			            INNER JOIN dbo.FingerMachineUsers AS FingerUser ON timesheet.UserNo = FingerUser.ID
			            INNER JOIN dbo.Requests AS request ON request.UserId = FingerUser.UserId
			            WHERE timesheet.UserNo IN (
					            SELECT ID
					            FROM dbo.FingerMachineUsers
					            )
				            AND (
					            timesheet.DayOfCheck BETWEEN request.StartDate
						            AND request.EndDate
					            )
				            AND (
					            request.RequestStatusId = 4
					            AND (
						            request.RequestTypeId = 5
						            OR request.RequestTypeId = 4
						            OR request.RequestTypeId = 2
						            OR request.RequestTypeId = 3
						            )
					            )
			            GROUP BY timesheet.ID
			            HAVING COUNT(*) > 1
			            )

	            DECLARE @TempTableTwoRequest TABLE (
		            UserNo INT
		            ,request1 INT
		            ,StartDate DATETIME
		            )

	            INSERT INTO @TempTableTwoRequest
	            SELECT DISTINCT t.UserNo
		            ,r.RequestTypeID AS request1
		            ,r.StartDate
	            FROM @MyTempTable6 t
	            INNER JOIN dbo.FingerMachineUsers fu ON t.UserNo = fu.ID
	            INNER JOIN dbo.Requests r ON r.UserId = fu.UserId
	            WHERE (
			            t.DayOfCheck BETWEEN r.StartDate
				            AND r.EndDate
			            )
		            AND (
			            r.RequestStatusId = 4
			            AND (
				            r.RequestTypeId = 5
				            OR r.RequestTypeId = 4
				            OR r.RequestTypeId = 2
				            OR r.RequestTypeId = 3
				            )
			            )

	            DECLARE @TableTwoRequest TABLE (
		            UserNo INT
		            ,request1 INT
		            ,request2 INT
		            ,StartDate DATETIME
		            )

	            INSERT INTO @TableTwoRequest
	            SELECT A.UserNo
		            ,MIN(A.request1)
		            ,MAX(A.request1)
		            ,A.StartDate
	            FROM @TempTableTwoRequest A
	            GROUP BY A.StartDate
		            ,A.UserNo

	            DECLARE @Table1Request TABLE (
		            Title NVARCHAR(256)
		            ,UserId NVARCHAR(128)
		            ,EntitleDayId INT
		            ,RequestTypeId INT
		            ,RequestStatusId INT
		            ,StartDate DATE
		            ,EndDate DATE
		            )

	            INSERT INTO @Table1Request
	            SELECT rq.Title
		            ,rq.UserId
		            ,rq.EntitleDayId
		            ,rq.RequestTypeId
		            ,rq.RequestStatusId
		            ,rq.StartDate
		            ,rq.EndDate
	            FROM dbo.Requests AS rq
	            INNER JOIN (
		            SELECT UserId
			            ,StartDate
			            ,EndDate
		            FROM dbo.Requests
		            WHERE RequestStatusId = 4
		            GROUP BY UserId
			            ,StartDate
			            ,EndDate
		            HAVING COUNT(RequestTypeId) = 1
		            ) AS abc ON abc.UserId = rq.UserId
	            WHERE abc.StartDate = rq.StartDate
		            AND abc.EndDate = rq.EndDate and rq.RequestStatusId = 4

	            INSERT INTO dbo.AbnormalCases (
		            TimeSheetId
		            ,ReasonId
		            ,STATUS
		            )
	            SELECT *
	            FROM (
		            SELECT DISTINCT TEMP.ID
			            ,(
				            CASE 
					            WHEN TEMP.Late IS NOT NULL
						            THEN 1
					            END
				            ) AS ReasonType
			            ,1 AS statuss
		            FROM @MyTempTable AS TEMP
		
		            UNION
		
		            SELECT DISTINCT TEMP.ID
			            ,(
				            CASE 
					            WHEN TEMP.LeaveEarly IS NOT NULL
						            THEN 2
					            END
				            ) AS ReasonType
			            ,1 AS statuss
		            FROM @MyTempTable AS TEMP
		
		            UNION
		
		            SELECT DISTINCT TEMP.ID
			            ,(
				            CASE 
					            WHEN TEMP.Absent IS NOT NULL
						            THEN 5
					            END
				            ) AS ReasonType
			            ,1 AS statuss
		            FROM @MyTempTable AS TEMP
		
		            UNION
		
		            SELECT DISTINCT TEMP.ID
			            ,(
				            CASE 
					            WHEN TEMP.OTCheckIn IS NULL
						            AND TEMP.OTcheckOut IS NULL
						            THEN 9
					            WHEN TEMP.OTCheckIn IS NULL
						            THEN 7
					            WHEN TEMP.OTcheckOut IS NULL
						            THEN 8
					            END
				            ) AS ReasonType
			            ,1 AS statuss
		            FROM @MyTempTable AS TEMP
		            INNER JOIN dbo.FingerMachineUsers AS fingUser ON TEMP.UserNo = fingUser.ID
		            INNER JOIN @OTUser AS ot ON fingUser.UserId = ot.UserId
			            AND ot.OTDate = TEMP.DayOfCheck
		            WHERE TEMP.OTCheckIn IS NULL
			            OR TEMP.OTcheckOut IS NULL
		
		            UNION
		
		            SELECT DISTINCT TEMP.ID
			            ,(
				            CASE 
					            WHEN TEMP.OTCheckIn IS NULL
						            AND TEMP.OTcheckOut IS NULL
						            THEN 9
					            WHEN TEMP.OTCheckIn IS NULL
						            THEN 7
					            WHEN TEMP.OTcheckOut IS NULL
						            THEN 8
					            END
				            ) AS ReasonType
			            ,1 AS statuss
		            FROM @MyTempTable5 AS TEMP
		            INNER JOIN dbo.FingerMachineUsers AS fingUser ON TEMP.UserNo = fingUser.ID
		            INNER JOIN @OTUser AS ot ON fingUser.UserId = ot.UserId
			            AND ot.OTDate = TEMP.DayOfCheck
		            WHERE TEMP.OTCheckIn IS NULL
			            OR TEMP.OTcheckOut IS NULL
		
		            UNION
		
		            SELECT DISTINCT TEMP.ID
			            ,(
				            CASE 
					            WHEN TEMP.Late IS NOT NULL
						            AND request.RequestTypeId <> 4
						            THEN 1
					            END
				            ) AS ReasonType
			            ,1 AS statuss
		            FROM @MyTempTable5 AS TEMP
		            INNER JOIN dbo.FingerMachineUsers AS fingeruser ON TEMP.UserNo = fingeruser.ID
		            INNER JOIN dbo.AppUsers AS appuser ON appuser.Id = fingeruser.UserId
		            LEFT JOIN dbo.Requests AS request ON request.UserId = fingeruser.UserId
		            WHERE request.UserId IS NULL
		
		            UNION
		
		            SELECT DISTINCT TEMP.ID
			            ,(
				            CASE 
					            WHEN TEMP.LeaveEarly IS NOT NULL
						            AND request.RequestTypeId <> 5
						            THEN 2
					            END
				            ) AS ReasonType
			            ,1 AS statuss
		            FROM @MyTempTable5 AS TEMP
		            INNER JOIN dbo.FingerMachineUsers AS fingeruser ON TEMP.UserNo = fingeruser.ID
		            INNER JOIN dbo.AppUsers AS appuser ON appuser.Id = fingeruser.UserId
		            LEFT JOIN dbo.Requests AS request ON request.UserId = fingeruser.UserId
		            WHERE request.UserId IS NULL
		
		            UNION
		
		            SELECT DISTINCT TEMP.ID
			            ,(
				            CASE 
					            WHEN request.RequestTypeId = 1
						            AND (
							            TEMP.CheckIn IS NOT NULL
							            OR TEMP.CheckOut IS NOT NULL
							            )
						            AND TEMP.NumOfWorking != '0'
						            THEN 6
					            WHEN request.RequestTypeId = 2
						            AND TEMP.Absent = 'VC'
						            THEN 6
					            WHEN request.RequestTypeId = 2
						            AND TEMP.Absent IS NULL
						            THEN 6
					            WHEN request.RequestTypeId = 3
						            AND TEMP.Absent = 'VS'
						            THEN 6
					            WHEN request.RequestTypeId = 3
						            AND TEMP.Absent IS NULL
						            THEN 6
					            END
				            ) AS ReasonType
			            ,1 AS statuss
		            FROM @MyTempTable5 AS TEMP
		            INNER JOIN dbo.FingerMachineUsers AS fingeruser ON TEMP.UserNo = fingeruser.ID
		            INNER JOIN dbo.AppUsers AS appuser ON appuser.Id = fingeruser.UserId
		            LEFT JOIN dbo.Requests AS request ON request.UserId = fingeruser.UserId
		            WHERE request.RequestStatusId = 4 AND (TEMP.DayOfCheck BETWEEN request.StartDate
				            AND request.EndDate)
		
		            UNION
		
		            SELECT DISTINCT TEMP.ID
			            ,(
				            CASE 
					            WHEN request.RequestTypeId = 2
						            AND TEMP.Absent = 'V'
						            THEN 5
					            WHEN request.RequestTypeId = 2
						            AND TEMP.Absent = 'VC'
						            THEN 5
					            WHEN request.RequestTypeId = 3
						            AND TEMP.Absent = 'VS'
						            THEN 5
					            WHEN request.RequestTypeId = 3
						            AND TEMP.Absent = 'V'
						            THEN 5
					            WHEN request.RequestTypeId = 4
						            AND TEMP.Absent = 'VC'
						            THEN 5
					            WHEN request.RequestTypeId = 4
						            AND TEMP.Absent = 'V'
						            THEN 5
					            WHEN request.RequestTypeId = 4
						            AND TEMP.Absent = 'VS'
						            THEN 5
					            WHEN request.RequestTypeId = 5
						            AND TEMP.Absent = 'VC'
						            THEN 5
					            WHEN request.RequestTypeId = 5
						            AND TEMP.Absent = 'V'
						            THEN 5
					            WHEN request.RequestTypeId = 5
						            AND TEMP.Absent = 'VS'
						            THEN 5
					            END
				            ) AS ReasonType
			            ,1 AS statuss
		            FROM @MyTempTable5 AS TEMP
		            INNER JOIN dbo.FingerMachineUsers AS fingeruser ON TEMP.UserNo = fingeruser.ID
		            INNER JOIN dbo.AppUsers AS appuser ON appuser.Id = fingeruser.UserId
		            INNER JOIN @Table1Request AS request ON request.UserId = fingeruser.UserId
		            WHERE TEMP.DayOfCheck BETWEEN request.StartDate
				            AND request.EndDate
		
		            UNION
		
		            SELECT DISTINCT TEMP.ID
			            ,(
				            CASE 
					            WHEN TEMP.LeaveEarly IS NOT NULL
						            AND request.RequestTypeId != 5
						            THEN 2
					            END
				            ) AS ReasonType
			            ,1 AS statuss
		            FROM @MyTempTable5 AS TEMP
		            INNER JOIN dbo.FingerMachineUsers AS fingeruser ON TEMP.UserNo = fingeruser.ID
		            INNER JOIN dbo.AppUsers AS appuser ON appuser.Id = fingeruser.UserId
		            INNER JOIN @Table1Request AS request ON request.UserId = fingeruser.UserId
		            WHERE TEMP.DayOfCheck BETWEEN request.StartDate
				            AND request.EndDate
		
		            UNION
		
		            SELECT DISTINCT TEMP.ID
			            ,(
				            CASE 
					            WHEN TEMP.Late IS NOT NULL
						            AND request.RequestTypeId != 4
						            THEN 1
					            END
				            ) AS ReasonType
			            ,1 AS statuss
		            FROM @MyTempTable5 AS TEMP
		            INNER JOIN dbo.FingerMachineUsers AS fingeruser ON TEMP.UserNo = fingeruser.ID
		            INNER JOIN dbo.AppUsers AS appuser ON appuser.Id = fingeruser.UserId
		            INNER JOIN @Table1Request AS request ON request.UserId = fingeruser.UserId
		            WHERE TEMP.DayOfCheck BETWEEN request.StartDate
				            AND request.EndDate
		
		            UNION
		
		            SELECT DISTINCT temp1.ID
			            ,(
				            CASE 
					            WHEN request.RequestTypeId = 4
						            AND temp1.Late IS NULL
						            AND (
							            temp1.Absent = 'VC'
							            OR temp1.Absent IS NULL
							            )
						            THEN 4
					            WHEN request.RequestTypeId = 5
						            AND temp1.LeaveEarly IS NULL
						            AND (
							            temp1.Absent = 'VS'
							            OR temp1.Absent IS NULL
							            )
						            THEN 3
					            END
				            ) AS ReasonType
			            ,1 AS statuss
		            FROM @MyTempTable5 AS temp1
		            INNER JOIN dbo.FingerMachineUsers AS fingeruser ON temp1.UserNo = fingeruser.ID
		            INNER JOIN dbo.AppUsers AS appuser ON appuser.Id = fingeruser.UserId
		            INNER JOIN @Table1Request AS request ON request.UserId = fingeruser.UserId
		            WHERE temp1.DayOfCheck BETWEEN request.StartDate
				            AND request.EndDate
		
		            UNION
		
		            SELECT DISTINCT temp1.ID
			            ,(
				            CASE 
					            WHEN request.RequestTypeId = 4
						            AND temp1.Late IS NULL
						            THEN 4
					            WHEN request.RequestTypeId = 5
						            AND temp1.LeaveEarly IS NULL
						            THEN 3
					            WHEN temp1.Absent IS NULL
						            AND request.RequestTypeId IS NOT NULL
						            THEN 6
					            WHEN temp1.OTCheckIn IS NULL
						            AND temp1.OTcheckOut IS NULL
						            THEN 9
					            WHEN temp1.OTCheckIn IS NULL
						            THEN 7
					            WHEN temp1.OTcheckOut IS NULL
						            THEN 8
					            END
				            ) AS ReasonType
			            ,1 AS statuss
		            FROM @MyTempTable1 AS temp1
		            INNER JOIN dbo.FingerMachineUsers AS fingeruser ON temp1.UserNo = fingeruser.ID
		            INNER JOIN dbo.AppUsers AS appuser ON appuser.Id = fingeruser.UserId
		            INNER JOIN dbo.Requests AS request ON request.UserId = fingeruser.UserId
		            INNER JOIN dbo.OTRequestUser AS otrequestuser ON otrequestuser.UserID = fingeruser.UserId
		            INNER JOIN dbo.OTRequests AS otrequest ON otrequest.ID = otrequestuser.OTRequestID
		            WHERE (
				            (
					            request.RequestStatusId = 4
					            AND otrequest.StatusRequestID = 4
					            AND request.UserId = fingeruser.UserId
					            AND (
						            temp1.DayOfCheck BETWEEN request.StartDate
							            AND request.EndDate
						            )
					            AND (temp1.DayOfCheck = otrequest.OTDate)
					            )
				            AND (
					            temp1.Absent IS NULL
					            OR temp1.Late IS NOT NULL
					            OR temp1.LeaveEarly IS NOT NULL
					            )
				            AND (
					            temp1.OTCheckIn IS NULL
					            OR temp1.OTcheckOut IS NULL
					            )
				            )
		
		            UNION
		
		            SELECT DISTINCT temp2.ID
			            ,(
				            CASE 
					            WHEN temp2.OTCheckIn IS NULL
						            AND temp2.OTcheckOut IS NULL
						            THEN 9
					            WHEN temp2.OTCheckIn IS NULL
						            THEN 7
					            WHEN temp2.OTcheckOut IS NULL
						            THEN 8
					            END
				            ) AS ReasonType
			            ,1 AS statuss
		            FROM @MyTempTable2 AS temp2
		            INNER JOIN dbo.FingerMachineUsers AS fingeruser ON temp2.UserNo = fingeruser.ID
		            INNER JOIN dbo.AppUsers AS appuser ON appuser.Id = fingeruser.UserId
		            INNER JOIN dbo.OTRequestUser AS otrequestuser ON otrequestuser.UserID = fingeruser.UserId
		            INNER JOIN dbo.OTRequests AS otrequest ON otrequest.ID = otrequestuser.OTRequestID
		            WHERE (
				            (
					            otrequest.StatusRequestID = 4
					            AND temp2.DayOfCheck = otrequest.OTDate
					            )
				            AND (
					            temp2.OTCheckIn IS NULL
					            OR temp2.OTcheckOut IS NULL
					            )
				            )
		
		            UNION
		
		            SELECT DISTINCT TEMP.ID
			            ,(
				            CASE 
					            WHEN TEMP.Late IS NOT NULL
						            AND (
							            table2request.request1 != 4
							            AND table2request.request2 != 4
							            )
						            THEN 1
					            END
				            ) AS ReasonType
			            ,1 AS statuss
		            FROM @MyTempTable6 AS TEMP
		            INNER JOIN @TableTwoRequest AS table2request ON table2request.UserNo = TEMP.UserNo
		            WHERE table2request.StartDate = TEMP.DayOfCheck
		
		            UNION
		
		            SELECT DISTINCT TEMP.ID
			            ,(
				            CASE 
					            WHEN TEMP.LeaveEarly IS NOT NULL
						            AND (
							            table2request.request1 != 5
							            AND table2request.request2 != 5
							            )
						            THEN 2
					            END
				            ) AS ReasonType
			            ,1 AS statuss
		            FROM @MyTempTable6 AS TEMP
		            INNER JOIN @TableTwoRequest AS table2request ON table2request.UserNo = TEMP.UserNo
		            WHERE table2request.StartDate = TEMP.DayOfCheck
		
		            UNION
		
		            SELECT DISTINCT TEMP.ID
			            ,(
				            CASE 
					            WHEN (
							            table2request.request1 = 4
							            AND table2request.request2 = 5
							            )
						            AND TEMP.CheckIn IS NOT NULL
						            AND TEMP.Checkout IS NULL
						            AND TEMP.LeaveEarly IS NULL
						            AND TEMP.Late IS NULL
						            AND TEMP.Absent = 'VC'
						            THEN 4
					            WHEN (
							            table2request.request1 = 4
							            AND table2request.request2 = 5
							            )
						            AND TEMP.CheckIn IS NOT NULL
						            AND TEMP.Checkout IS NOT NULL
						            AND TEMP.LeaveEarly IS NULL
						            AND TEMP.Late IS NULL
						            AND TEMP.Absent = 'VC'
						            THEN 4
					            WHEN (
							            table2request.request1 = 4
							            AND table2request.request2 = 5
							            )
						            AND TEMP.CheckIn IS NOT NULL
						            AND TEMP.Checkout IS NOT NULL
						            AND TEMP.LeaveEarly IS NOT NULL
						            AND TEMP.Late IS NULL
						            AND TEMP.Absent IS NULL
						            AND TEMP.NumOfWorking = '1'
						            THEN 4
					            WHEN (
							            table2request.request1 = 4
							            AND table2request.request2 = 5
							            )
						            AND TEMP.CheckIn IS NOT NULL
						            AND TEMP.Checkout IS NOT NULL
						            AND TEMP.LeaveEarly IS NULL
						            AND TEMP.Late IS NULL
						            AND TEMP.Absent IS NULL
						            AND TEMP.NumOfWorking = '1'
						            THEN 4
					            WHEN (
							            table2request.request1 = 3
							            AND table2request.request2 = 4
							            )
						            AND TEMP.CheckIn IS NOT NULL
						            AND TEMP.Checkout IS NULL
						            AND TEMP.LeaveEarly IS NULL
						            AND TEMP.Late IS NULL
						            AND TEMP.Absent = 'VC'
						            AND TEMP.NumOfWorking = '0.5'
						            THEN 4
					            WHEN (
							            table2request.request1 = 3
							            AND table2request.request2 = 4
							            )
						            AND TEMP.CheckIn IS NOT NULL
						            AND TEMP.Checkout IS NOT NULL
						            AND TEMP.LeaveEarly IS NOT NULL
						            AND TEMP.Late IS NULL
						            AND TEMP.Absent IS NULL
						            AND TEMP.NumOfWorking = '1'
						            THEN 4
					            WHEN (
							            table2request.request1 = 3
							            AND table2request.request2 = 4
							            )
						            AND TEMP.CheckIn IS NOT NULL
						            AND TEMP.Checkout IS NOT NULL
						            AND TEMP.LeaveEarly IS NULL
						            AND TEMP.Late IS NULL
						            AND TEMP.Absent = 'VC'
						            AND TEMP.NumOfWorking = '0.5'
						            THEN 4
					            WHEN (
							            table2request.request1 = 3
							            AND table2request.request2 = 4
							            )
						            AND TEMP.CheckIn IS NOT NULL
						            AND TEMP.Checkout IS NOT NULL
						            AND TEMP.LeaveEarly IS NULL
						            AND TEMP.Late IS NULL
						            AND TEMP.Absent IS NULL
						            AND TEMP.NumOfWorking = '1'
						            THEN 4
					            WHEN (
							            table2request.request1 = 3
							            AND table2request.request2 = 4
							            )
						            AND TEMP.CheckIn IS NOT NULL
						            AND TEMP.Checkout IS NULL
						            AND TEMP.LeaveEarly IS NULL
						            AND TEMP.Late IS NULL
						            AND TEMP.Absent = 'V'
						            AND TEMP.NumOfWorking = '0'
						            THEN 4
					            END
				            ) AS ReasonType
			            ,1 AS statuss
		            FROM @MyTempTable6 AS TEMP
		            INNER JOIN @TableTwoRequest AS table2request ON table2request.UserNo = TEMP.UserNo
		            WHERE table2request.StartDate = TEMP.DayOfCheck
		
		            UNION
		
		            SELECT DISTINCT TEMP.ID
			            ,(
				            CASE 
					            WHEN (
							            table2request.request1 = 4
							            AND table2request.request2 = 5
							            )
						            AND TEMP.LeaveEarly IS NULL
						            AND TEMP.Late IS NOT NULL
						            AND TEMP.Absent IS NULL
						            THEN 3
					            WHEN (
							            table2request.request1 = 4
							            AND table2request.request2 = 5
							            )
						            AND TEMP.LeaveEarly IS NULL
						            AND TEMP.Late IS NULL
						            AND TEMP.Absent = 'VS'
						            THEN 3
					            WHEN (
							            table2request.request1 = 4
							            AND table2request.request2 = 5
							            )
						            AND TEMP.CheckIn IS NOT NULL
						            AND TEMP.Checkout IS NOT NULL
						            AND TEMP.LeaveEarly IS NULL
						            AND TEMP.Late IS NULL
						            AND TEMP.Absent IS NULL
						            AND TEMP.NumOfWorking = '1'
						            THEN 3
					            WHEN (
							            table2request.request1 = 2
							            AND table2request.request2 = 5
							            )
						            AND (TEMP.CheckIn IS NOT NULL)
						            AND TEMP.CheckOut IS NOT NULL
						            AND TEMP.LeaveEarly IS NULL
						            AND TEMP.Absent = 'VS'
						            THEN 3
					            WHEN (
							            table2request.request1 = 2
							            AND table2request.request2 = 5
							            )
						            AND (TEMP.CheckIn IS NULL)
						            AND TEMP.CheckOut IS NOT NULL
						            AND TEMP.LeaveEarly IS NULL
						            AND TEMP.Absent = 'VS'
						            THEN 3
					            WHEN (
							            table2request.request1 = 2
							            AND table2request.request2 = 5
							            )
						            AND (TEMP.CheckIn IS NOT NULL)
						            AND TEMP.CheckOut IS NOT NULL
						            AND TEMP.LeaveEarly IS NULL
						            AND TEMP.Late IS NULL
						            AND TEMP.Absent IS NULL
						            THEN 3
					            WHEN (
							            table2request.request1 = 2
							            AND table2request.request2 = 5
							            )
						            AND (TEMP.CheckIn IS NOT NULL)
						            AND TEMP.CheckOut IS NOT NULL
						            AND TEMP.LeaveEarly IS NULL
						            AND TEMP.Late IS NOT NULL
						            AND TEMP.Absent IS NULL
						            THEN 3
					            END
				            ) AS ReasonType
			            ,1 AS statuss
		            FROM @MyTempTable6 AS TEMP
		            INNER JOIN @TableTwoRequest AS table2request ON table2request.UserNo = TEMP.UserNo
		            WHERE table2request.StartDate = TEMP.DayOfCheck
		
		            UNION
		
		            SELECT DISTINCT TEMP.ID
			            ,(
				            CASE 
					            WHEN (
							            table2request.request1 = 2
							            AND table2request.request2 = 5
							            )
						            AND (TEMP.CheckIn IS NOT NULL)
						            AND TEMP.CheckOut IS NOT NULL
						            AND TEMP.Absent = 'VC'
						            THEN 5
					            WHEN (
							            table2request.request1 = 2
							            AND table2request.request2 = 5
							            )
						            AND (TEMP.CheckIn IS NOT NULL)
						            AND TEMP.CheckOut IS NULL
						            AND TEMP.Absent = 'VC'
						            THEN 5
					            WHEN (
							            table2request.request1 = 2
							            AND table2request.request2 = 5
							            )
						            AND (TEMP.CheckIn IS NOT NULL)
						            AND TEMP.CheckOut IS NULL
						            AND TEMP.Absent = 'V'
						            THEN 5
					            WHEN (
							            table2request.request1 = 2
							            AND table2request.request2 = 5
							            )
						            AND (TEMP.CheckIn IS NOT NULL)
						            AND TEMP.CheckOut IS NOT NULL
						            AND TEMP.Absent = 'V'
						            THEN 5
					            WHEN (
							            table2request.request1 = 2
							            AND table2request.request2 = 5
							            )
						            AND (TEMP.CheckIn IS NULL)
						            AND TEMP.CheckOut IS NULL
						            AND TEMP.Absent = 'V'
						            THEN 5
					            WHEN (
							            table2request.request1 = 3
							            AND table2request.request2 = 4
							            )
						            AND (TEMP.CheckIn IS NOT NULL)
						            AND TEMP.CheckOut IS NULL
						            AND TEMP.Absent = 'V'
						            THEN 5
					            WHEN (
							            table2request.request1 = 3
							            AND table2request.request2 = 4
							            )
						            AND (TEMP.CheckIn IS NULL)
						            AND TEMP.CheckOut IS NULL
						            AND TEMP.Absent = 'V'
						            THEN 5
					            WHEN (
							            table2request.request1 = 3
							            AND table2request.request2 = 4
							            )
						            AND (TEMP.CheckIn IS NULL)
						            AND TEMP.CheckOut IS NOT NULL
						            AND TEMP.Absent = 'VS'
						            THEN 5
					            WHEN (
							            table2request.request1 = 3
							            AND table2request.request2 = 4
							            )
						            AND (TEMP.CheckIn IS NOT NULL)
						            AND TEMP.CheckOut IS NOT NULL
						            AND TEMP.Absent = 'V'
						            THEN 5
					            WHEN (
							            table2request.request1 = 3
							            AND table2request.request2 = 4
							            )
						            AND (TEMP.CheckIn IS NOT NULL)
						            AND TEMP.CheckOut IS NOT NULL
						            AND TEMP.Late IS NULL
						            AND TEMP.LeaveEarly IS NOT NULL
						            AND TEMP.Absent = 'VS'
						            THEN 5
					            WHEN (
							            table2request.request1 = 3
							            AND table2request.request2 = 4
							            )
						            AND (TEMP.CheckIn IS NOT NULL)
						            AND TEMP.CheckOut IS NOT NULL
						            AND TEMP.Late IS NULL
						            AND TEMP.LeaveEarly IS NULL
						            AND TEMP.Absent = 'VS'
						            THEN 5
					            WHEN (
							            table2request.request1 = 4
							            AND table2request.request2 = 5
							            )
						            AND TEMP.CheckIn IS NOT NULL
						            AND TEMP.CheckOut IS NOT NULL
						            AND TEMP.Absent IS NOT NULL
						            THEN 5
					            WHEN (
							            table2request.request1 = 4
							            AND table2request.request2 = 5
							            )
						            AND TEMP.CheckIn IS NOT NULL
						            AND TEMP.CheckOut IS NULL
						            THEN 5
					            WHEN (
							            table2request.request1 = 4
							            AND table2request.request2 = 5
							            )
						            AND TEMP.CheckIn IS NULL
						            AND TEMP.CheckOut IS NULL
						            AND TEMP.Absent IS NOT NULL
						            THEN 5
					            WHEN (
							            table2request.request1 = 4
							            AND table2request.request2 = 5
							            )
						            AND TEMP.CheckIn IS NULL
						            AND TEMP.CheckOut IS NOT NULL
						            AND TEMP.Absent = 'VS'
						            THEN 5
					            END
				            ) AS ReasonType
			            ,1 AS statuss
		            FROM @MyTempTable6 AS TEMP
		            INNER JOIN @TableTwoRequest AS table2request ON table2request.UserNo = TEMP.UserNo
		            WHERE table2request.StartDate = TEMP.DayOfCheck
		
		            UNION
		
		            SELECT DISTINCT TEMP.ID
			            ,(
				            CASE 
					            WHEN (
							            table2request.request1 = 1
							            OR table2request.request2 = 1
							            )
						            AND (
							            TEMP.CheckIn IS NOT NULL
							            OR TEMP.CheckOut IS NOT NULL
							            )
						            THEN 6
					            WHEN (
							            table2request.request1 = 2
							            OR table2request.request2 = 2
							            )
						            AND TEMP.CheckIn IS NOT NULL
						            AND TEMP.CheckOut IS NOT NULL
						            AND (
							            TEMP.Absent = 'VC'
							            OR TEMP.Absent IS NULL
							            )
						            THEN 6
					            WHEN (
							            table2request.request1 = 2
							            OR table2request.request2 = 2
							            )
						            AND TEMP.CheckIn IS NOT NULL
						            AND TEMP.CheckOut IS NULL
						            AND TEMP.Absent = 'VC'
						            THEN 6
					            WHEN (
							            table2request.request1 = 2
							            AND table2request.request2 = 3
							            )
						            AND TEMP.CheckIn IS NULL
						            AND TEMP.CheckOut IS NOT NULL
						            AND TEMP.Late IS NULL
						            AND TEMP.LeaveEarly IS NOT NULL
						            AND TEMP.Absent IS NOT NULL
						            THEN 6
					            WHEN (
							            table2request.request1 = 2
							            AND table2request.request2 = 3
							            )
						            AND TEMP.CheckIn IS NULL
						            AND TEMP.CheckOut IS NOT NULL
						            AND TEMP.Late IS NULL
						            AND TEMP.LeaveEarly IS NULL
						            AND TEMP.Absent IS NOT NULL
						            THEN 6
					            WHEN (
							            table2request.request1 = 3
							            AND table2request.request2 = 4
							            )
						            AND TEMP.CheckIn IS NOT NULL
						            AND TEMP.CheckOut IS NOT NULL
						            AND TEMP.Late IS NOT NULL
						            AND TEMP.LeaveEarly IS NOT NULL
						            AND TEMP.Absent IS NULL
						            THEN 6
					            WHEN (
							            table2request.request1 = 3
							            AND table2request.request2 = 4
							            )
						            AND TEMP.CheckIn IS NOT NULL
						            AND TEMP.CheckOut IS NOT NULL
						            AND TEMP.Late IS NOT NULL
						            AND TEMP.LeaveEarly IS NULL
						            AND TEMP.Absent IS NULL
						            AND TEMP.NumOfWorking = '1'
						            THEN 6
					            WHEN (
							            table2request.request1 = 3
							            AND table2request.request2 = 4
							            )
						            AND (TEMP.CheckIn IS NOT NULL)
						            AND TEMP.CheckOut IS NOT NULL
						            AND TEMP.Late IS NULL
						            AND TEMP.LeaveEarly IS NOT NULL
						            AND TEMP.Absent = 'VS'
						            THEN 6
					            WHEN (
							            table2request.request1 = 3
							            AND table2request.request2 = 4
							            )
						            AND (TEMP.CheckIn IS NOT NULL)
						            AND TEMP.CheckOut IS NOT NULL
						            AND TEMP.Late IS NULL
						            AND TEMP.LeaveEarly IS NULL
						            AND TEMP.Absent = 'VS'
						            THEN 6
					            WHEN (
							            table2request.request1 = 3
							            AND table2request.request2 = 4
							            )
						            AND (TEMP.CheckIn IS NULL)
						            AND TEMP.CheckOut IS NOT NULL
						            AND TEMP.Late IS NULL
						            AND TEMP.LeaveEarly IS NOT NULL
						            AND TEMP.Absent = 'VS'
						            THEN 6
					            WHEN (
							            table2request.request1 = 3
							            AND table2request.request2 = 4
							            )
						            AND (TEMP.CheckIn IS NULL)
						            AND TEMP.CheckOut IS NOT NULL
						            AND TEMP.Late IS NULL
						            AND TEMP.LeaveEarly IS NULL
						            AND TEMP.Absent = 'VS'
						            THEN 6
					            END
				            ) AS ReasonType
			            ,1 AS statuss
		            FROM @MyTempTable6 AS TEMP
		            INNER JOIN @TableTwoRequest AS table2request ON table2request.UserNo = TEMP.UserNo
		            WHERE table2request.StartDate = TEMP.DayOfCheck
		            ) AS table1
	            WHERE table1.ReasonType IS NOT NULL
		            AND NOT EXISTS (
			            SELECT *
			            FROM dbo.AbnormalCases AS ab
			            WHERE ab.TimeSheetID = table1.ID
			            )
            END ";

        public const string ExecuteStoreAbnormalCaseQuery = "EXEC USP_Abnormal @StartDate = @minDate,  @EndDate = @maxDate";
        public const string CreateStoreAbnormalReasonQuery = "CREATE PROC USP_AbnormalCaseReason " +
            " AS" +
            " BEGIN" +

            " INSERT INTO dbo.AbnormalCaseReasons(AbnormalId, AbnormalReasonId,[Status] )" +
            " SELECT ab.Id, ab.ReasonId,1" +

            " FROM AbnormalCases ab" +
            " END";
        public const string ExecuteStoreAbnormalReasonQuery = "EXEC USP_AbnormalCaseReason";
        public const string TriggerRequest = "CREATE TRIGGER dbo.TG_Requests ON dbo.Requests AFTER UPDATE AS BEGIN  SET NOCOUNT ON;  EXEC USP_Abnormal ; EXEC USP_REPORT ;  END";
        public const string TriggerOTRequest = "CREATE TRIGGER dbo.TG_OTRequests ON dbo.OTRequests AFTER UPDATE AS BEGIN  SET NOCOUNT ON;  EXEC USP_Abnormal  END";
        public const string StoreAbnormalDayOff = " CREATE PROC USP_AB1"
        + "   AS"
        + "    BEGIN"
        + "    DECLARE @MyTempTable3 TABLE(ID INT, UserNo int, DayOfCheck datetime, OTCheckIn nvarchar(50),OTcheckOut nvarchar(50))"
        + "    INSERT INTO @MyTempTable3"
        + "    SELECT timesheet.ID,timesheet.UserNo,timesheet.DayOfCheck,timesheet.OTCheckIn,timesheet.OTCheckOut"
        + "    FROM dbo.FingerTimeSheets as timesheet"
        + "    WHERE Absent is NULL or Late is NOT NULL or LeaveEarly is NOT NULL"
        + "    DECLARE @OTUser1 TABLE(UserId nvarchar(50),OTDate datetime, Title nvarchar(256))"
        + "    INSERT INTO @OTUser1"
        + "    SELECT otuser.UserID,otrequets.OTDate,otrequets.Title FROM dbo.OTRequests as otrequets"
        + "    JOIN dbo.OTRequestUser as otuser"
        + "    ON otrequets.ID = otuser.OTRequestID"
        + "    WHERE otrequets.StatusRequestID =4"
        + "    INSERT INTO dbo.AbnormalCases (TimeSheetID,ReasonId,Status)"
        + "    Select * from ( SELECT temp3.ID,(CASE"
        + "    WHEN temp3.OTCheckIn is NULL and temp3.OTcheckOut IS NULL THEN 9"
        + "    WHEN temp3.OTCheckIn is NULL THEN 7"
        + "    WHEN temp3.OTcheckOut is NULL THEN 8 END)  as ReasonType,1 as statuss"
        + "    FROM @MyTempTable3 as temp3"
        + "    JOIn dbo.FingerMachineUsers as finger"
        + "    ON finger.ID = temp3.UserNo"
        + "    Right JOIN @OTUser1 as ot"
        + "    ON ot.UserId = finger.UserId"
        + "    WHere  ot.OTDate = temp3.DayOfCheck and NOT EXISTS(SELECT* FROM dbo.AbnormalCases as ab WHERE ab.TimeSheetID = temp3.ID)) as table4 WHERE table4.ReasonType IS NOT NULL"
        + "  END";

        public const string ExcuteStoreAbnormalDayOff = "EXEC USP_AB1";

        public const string StoreProcedureCheckTimeOut = " Create PROC CheckTime"
          + "   AS"
          + "   BEGIN"
          + "   UPDATE dbo.OTRequests SET StatusRequestID = 2 WHERE ID IN (SELECT ID FROM dbo.OTRequests where OTDate  <  (SELECT DATEADD(dd, 0, DATEDIFF(dd, 0, GETDATE()))) and StatusRequestID = 1)"
          + "   UPDATE dbo.Requests SET RequestStatusId = 2 WHERE ID IN (SELECT ID FROM dbo.Requests where StartDate < (SELECT DATEADD(dd, 0, DATEDIFF(dd, 0, GETDATE()))) and(RequestStatusId = 1 or RequestStatusId = 5))"
          + "   END";
        public const string ExcuteCheckTimeOut = " EXEC CheckTime";
        public const string StoreProcedureReport = " CREATE PROC USP_REPORT "
         + "   AS"
         + "   BEGIN"
+ "              DECLARE @TimeTable TABLE(TimeSheetID int, DateCheckRequest Datetime, LeaveMounth float)                                            "
+ "                   INSERT INTO @TimeTable                                                                                                        "
+ "                   SELECT DISTINCT timesheet.ID, timesheet.DayOfCheck ,(CASE                                                                     "
//           --        WHEN timesheet.Absent = 'V' AND (timesheet.CheckIn IS NOT NULL OR timesheet.CheckOut IS NOT NULL) THEN 0
//           --        WHEN timesheet.Absent ='VC' AND (timesheet.CheckIn IS NOT NULL OR timesheet.CheckOut IS NOT NULL) THEN 0
//           --        WHEN timesheet.Absent ='VS' AND (timesheet.CheckIn IS NOT NULL OR timesheet.CheckOut IS NOT NULL) THEN 0
+ "                   WHEN timesheet.Absent = 'V' THEN 1                                                                                            "
+ "                   WHEN timesheet.Absent = 'VC' THEN 0.5                                                                                         "
+ "                   WHEN timesheet.Absent = 'VS' THEN 0.5                                                                                         "
+ "                   END) as LeaveMounth                                                                                                           "
+ "                   FROM dbo.FingerTimeSheets as timesheet                                                                                        "
+ "                   JOIN dbo.FingerMachineUsers as FingerUser                                                                                     "
+ "                   ON timesheet.UserNo = FingerUser.ID                                                                                           "
+ "                   JOIN dbo.Requests as request                                                                                                  "
+ "                   ON request.UserId = FingerUser.UserId                                                                                         "
+ "                   WHERE(Absent is NOT NULL)                                                                                                     "
+ "                   and(timesheet.DayOfCheck between request.StartDate and request.EndDate)                                                       "
+ "                   and(request.RequestStatusId = 4 and request.EntitleDayId = 1                                                                  "
+ "                   and(request.RequestTypeId= 1))                                                                                                "
+ "                UNION                                                                                                                            "
+ "                   SELECT DISTINCT timesheet.ID, timesheet.DayOfCheck ,(CASE                                                                     "
+ "                   WHEN timesheet.Absent = 'V' THEN 0.5                                                                                          "
+ "                   WHEN timesheet.Absent = 'VS' THEN 0.5                                                                                         "
//            --phuc add them
+ "                   WHEN timesheet.Absent = 'VC' THEN 0                                                                                           "
+ "                   END) as LeaveMounth                                                                                                           "
+ "                   FROM dbo.FingerTimeSheets as timesheet                                                                                        "
+ "                   JOIN dbo.FingerMachineUsers as FingerUser                                                                                     "
+ "                   ON timesheet.UserNo = FingerUser.ID                                                                                           "
+ "                   JOIN dbo.Requests as request                                                                                                  "
+ "                   ON request.UserId = FingerUser.UserId                                                                                         "
+ "                   WHERE(Absent is NOT NULL)                                                                                                     "
+ "                   and(timesheet.DayOfCheck between request.StartDate and request.EndDate)                                                       "
+ "                   and(request.RequestStatusId = 4 and request.EntitleDayId = 1                                                                  "
+ "                   and(request.RequestTypeId= 2) )                                                                                               "
+ "                UNION                                                                                                                            "
+ "                   SELECT DISTINCT timesheet.ID, timesheet.DayOfCheck ,(CASE                                                                     "
+ "                   WHEN timesheet.Absent = 'V' THEN 0.5                                                                                          "
+ "                   WHEN timesheet.Absent = 'VC' THEN 0.5                                                                                         "
//            --phuc add them
+ "                   WHEN timesheet.Absent = 'VS' THEN 0                                                                                           "
+ "                   END) as LeaveMounth                                                                                                           "
+ "                   FROM dbo.FingerTimeSheets as timesheet                                                                                        "
+ "                   JOIN dbo.FingerMachineUsers as FingerUser                                                                                     "
+ "                   ON timesheet.UserNo = FingerUser.ID                                                                                           "
+ "                   JOIN dbo.Requests as request                                                                                                  "
+ "                   ON request.UserId = FingerUser.UserId                                                                                         "
+ "                   WHERE(Absent is NOT NULL)                                                                                                     "
+ "                   and(timesheet.DayOfCheck between request.StartDate and request.EndDate)                                                       "
+ "                   and(request.RequestStatusId = 4 and request.EntitleDayId = 1                                                                  "
+ "                   and(request.RequestTypeId= 3))                                                                                                "
+ "				   UNION                                                                                                                            "
//				   --tets
+ "                   SELECT DISTINCT timesheet.ID, timesheet.DayOfCheck ,(CASE                                                                     "
+ "                   WHEN timesheet.Absent = 'V' THEN 1                                                                                            "
+ "                   WHEN timesheet.Absent = 'VC' THEN 0.5                                                                                         "
+ "                   WHEN timesheet.Absent = 'VS' THEN 0.5                                                                                         "
+ "                   END) as LeaveMounth                                                                                                           "
+ "                   FROM dbo.FingerTimeSheets as timesheet                                                                                        "
+ "                   JOIN dbo.FingerMachineUsers as FingerUser                                                                                     "
+ "                   ON timesheet.UserNo = FingerUser.ID                                                                                           "
+ "                   JOIN dbo.ExplanationRequests as ex                                                                                            "
+ "                   ON ex.TimeSheetId = timesheet.ID                                                                                              "
+ "                   WHERE(Absent is NOT NULL) and ex.Actual ='Leave'                                                                  "
+ "				   and ex.StatusRequestId = 4                                                                                                       "
+ "                  and timesheet.ID not in( SELECT DISTINCT finger.ID FROM dbo.FingerTimeSheets as finger                                         "
+ "                  JOIN dbo.FingerMachineUsers as finguser                                                                                        "
+ "                  on finger.UserNo = finguser.ID                                                                                                 "
+ "                  join dbo.Requests as re                                                                                                        "
+ "                  ON re.UserId = finguser.UserId                                                                                                 "
+ "                  WHERE (finger.DayOfCheck between re.StartDate and re.EndDate)                                                                  "
+ "                  and re.RequestStatusId =4 and re.EntitleDayId is not null                                                                      "
+ "                  and (re.RequestTypeId = 1 or re.RequestTypeId = 2 or re.RequestTypeId = 3))                                                    "
//				--end test                                                                                                                          "
+ "                  INSERT INTO dbo.Reports(TimeSheetID, LeaveMounth, DateCheckRequest, Status)                                                    "
+ "           	  SELECT tempReport.TimeSheetID, tempReport.LeaveMounth, tempReport.DateCheckRequest, 1 as Status FROM @TimeTable as tempReport     "
+ "              WHERE NOT EXISTS (SELECT* FROM dbo.Reports WHERE TimeSheetID = tempReport.TimeSheetID)                                             "
         + "   END";
        public const string ExcuteReport = " EXEC USP_REPORT";
    }
}
