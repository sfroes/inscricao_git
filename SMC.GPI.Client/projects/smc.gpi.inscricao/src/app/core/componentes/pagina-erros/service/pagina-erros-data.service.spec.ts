import { TestBed } from '@angular/core/testing';

import { PaginaErrosDataService } from './pagina-erros-data.service';

describe('PaginaErrosDataService', () => {
  let service: PaginaErrosDataService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(PaginaErrosDataService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
