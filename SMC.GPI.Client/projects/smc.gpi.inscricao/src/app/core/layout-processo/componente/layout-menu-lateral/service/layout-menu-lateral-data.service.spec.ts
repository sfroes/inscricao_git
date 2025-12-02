import { TestBed } from '@angular/core/testing';

import { LayoutMenuLateralDataService } from './layout-menu-lateral-data.service';

describe('LayoutMenuLateralDataService', () => {
  let service: LayoutMenuLateralDataService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(LayoutMenuLateralDataService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
