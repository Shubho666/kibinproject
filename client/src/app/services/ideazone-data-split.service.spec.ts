import { TestBed } from '@angular/core/testing';

import { IdeazoneDataSplitService } from './ideazone-data-split.service';

describe('IdeazoneDataSplitService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: IdeazoneDataSplitService = TestBed.get(IdeazoneDataSplitService);
    expect(service).toBeTruthy();
  });
});
