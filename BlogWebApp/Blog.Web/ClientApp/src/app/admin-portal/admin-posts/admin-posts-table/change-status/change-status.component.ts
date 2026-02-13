import { Component, OnInit, Input, ContentChild, ElementRef, Output, EventEmitter, ChangeDetectionStrategy } from '@angular/core';
import { Messages } from './../../../../core/data/Mesages';
import { CustomToastrService } from './../../../../core/services/custom-toastr.service';

@Component({
  selector: 'app-change-status',
  templateUrl: './change-status.component.html',
  styleUrls: ['./change-status.component.css'],
  standalone: false,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ChangeStatusComponent implements OnInit {
  /**
   * @param status any
   */
  @Input('status') status: any;

  /**
   * @param onChanged EventEmitter<string>
   */
  @Output() onChanged: EventEmitter<string> = new EventEmitter<string>();

  /**
   * @param button ElementRef
   */
  @ContentChild('button') button: ElementRef | undefined;

  /**
   * @param changed boolean
   */
  changed = false;

  /**
   * @param newStatus string
   */
  newStatus = '';

  /**
   * @param _customToastrService CustomToastrService
   */
  constructor(private _customToastrService: CustomToastrService) {
  }

  /**
   * @inheritdoc
   */
  ngOnInit() {
  }

  /**
   * Change select value.
   * @param newStatus any
   */
  changeStatus(newStatus: any) {
    if (this.status !== newStatus) {
      this.changed = true;
      this.newStatus = newStatus;
      this._customToastrService.displaySuccessMessage(Messages.POST_EDITED_SUCCESSFULLY);
    }
  }

  /**
   * Change status.
   */
  change() {
    this.onChanged.emit(this.newStatus);
  }
}
