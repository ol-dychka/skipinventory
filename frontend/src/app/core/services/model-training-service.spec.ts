import { TestBed } from '@angular/core/testing';

import { ModelTrainingService } from './model-training-service';

describe('ModelTrainingService', () => {
  let service: ModelTrainingService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ModelTrainingService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
