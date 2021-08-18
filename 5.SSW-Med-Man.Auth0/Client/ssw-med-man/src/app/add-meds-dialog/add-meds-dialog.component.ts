import { Component, OnInit, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MedicationDTO } from 'src/helpers/api-client';
import { MedicationsComponent } from '../medications/medications.component';

@Component({
  selector: 'app-add-meds-dialog',
  templateUrl: './add-meds-dialog.component.html',
  styleUrls: ['./add-meds-dialog.component.css']
})
export class AddMedsDialogComponent implements OnInit {

  constructor(
    public dialogRef: MatDialogRef<MedicationsComponent>,
    @Inject(MAT_DIALOG_DATA) public data: MedicationDTO) {}

  onNoClick(): void {
    this.dialogRef.close();
  }

  ngOnInit() {
  }

}
