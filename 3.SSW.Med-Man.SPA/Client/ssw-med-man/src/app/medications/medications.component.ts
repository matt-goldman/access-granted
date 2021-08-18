import { Component, OnInit,Input } from '@angular/core';
import { MedicationDTO, MedicationsClient } from '../../helpers/api-client';
import { MatDialog } from '@angular/material/dialog';
import { AddMedsDialogComponent } from '../add-meds-dialog/add-meds-dialog.component';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Subject } from 'rxjs';

@Component({
  selector: 'app-medications',
  templateUrl: './medications.component.html',
  styleUrls: ['./medications.component.css']
})
export class MedicationsComponent implements OnInit {

  constructor(private medsClient: MedicationsClient, public dialog: MatDialog, private _snackBar: MatSnackBar) { }

  displayedColumns: string[] = ['name'];

  private _medList = new Subject();
  
  medications = this._medList.asObservable();

  private data: MedicationDTO[];

  newMed: MedicationDTO = {};

  areMedications: boolean;

  ngOnInit() {
    this.medsClient.getMedications()
    .subscribe(
      (result) => {
        this.data = result;
        this.notify();
        this.areMedications = this.data.length > 0;
      });
  }

  ngAfterViewInit() {
    this.medsClient.getMedications()
    .subscribe(
      (result) => {
        this.data = result;
        this.notify();
        this.areMedications = this.data.length > 0;
      });
  }
  
  openDialog(): void {
    const dialogRef = this.dialog.open(AddMedsDialogComponent, {
      width: '250px',
      data: { }
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log('The dialog was closed');
      this.newMed.name = result;
      if(result != null) {
        console.log("Adding this medication:");
        console.log(this.newMed);
        this.medsClient.postMedication(this.newMed)
        .subscribe(
          result => {
            console.log("Result:");
            console.log(result);
            this.data.push(result);
            this.notify();
            this.areMedications = this.data.length > 0;
            this._snackBar.open('Medication added', 'OK', {duration: 3000});
          },
          error => {
            console.log(error);
            this._snackBar.open('Adding medication failed', 'OK', {duration: 3000});
        });
      }
    });
  }

  notify() {
    console.log("Updating list...");
    console.log(this.data);
    this._medList.next(this.data);
  }
}
