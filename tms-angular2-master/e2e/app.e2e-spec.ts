import { TMSAngular2Page } from './app.po';

describe('tms-angular2 App', () => {
  let page: TMSAngular2Page;

  beforeEach(() => {
    page = new TMSAngular2Page();
  });

  it('should display message saying app works', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('app works!');
  });
});
