import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Watchlists } from './watchlists';

describe('Watchlists', () => {
  let component: Watchlists;
  let fixture: ComponentFixture<Watchlists>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Watchlists]
    })
    .compileComponents();

    fixture = TestBed.createComponent(Watchlists);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
