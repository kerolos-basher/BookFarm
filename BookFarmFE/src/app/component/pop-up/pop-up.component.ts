import { Component, Inject } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';

import { MAT_DIALOG_DATA,MatDialogRef  } from '@angular/material/dialog';

@Component({
  selector: 'app-pop-up',
  standalone: true,
  imports: [],
  templateUrl: './pop-up.component.html',
  styleUrl: './pop-up.component.scss'
})
export class PopUpComponent {
  constructor(
    @Inject(MAT_DIALOG_DATA) public data: {sign: string; message: string ;color:string },
    private dialogRef: MatDialogRef<PopUpComponent>
  ) {}
  closeDialog(): void {
    this.dialogRef.close();
  }
}
