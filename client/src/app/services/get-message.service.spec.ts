import { TestBed } from '@angular/core/testing';

import { GetMessageService } from './get-message.service';

describe('GetMessageService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: GetMessageService = TestBed.get(GetMessageService);
    expect(service).toBeTruthy();
  });
});
