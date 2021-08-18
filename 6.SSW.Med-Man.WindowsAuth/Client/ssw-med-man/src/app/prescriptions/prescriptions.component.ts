import { Component, OnInit } from '@angular/core';
import { PrescriptionsClient, PrescriptionDTO } from '../../helpers/api-client';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Subject } from 'rxjs';

@Component({
  selector: 'app-prescriptions',
  templateUrl: './prescriptions.component.html',
  styleUrls: ['./prescriptions.component.css']
})
export class PrescriptionsComponent implements OnInit {

  constructor(private scriptsClient: PrescriptionsClient, private dialog: MatDialog, private snackBar: MatSnackBar) { }

  displayedColumns: string[] = ['patientName', 'medication','dose'];

  prescriptions: PrescriptionDTO[] = [];

  newScript: PrescriptionDTO = {id: null, medication: null, patient: null, dose: null};

  arePrescriptions: boolean;

  ngOnInit() {
    this.scriptsClient.getPrescriptions()
    .subscribe(
      (result) => {
        this.prescriptions = result;
        this.arePrescriptions = this.prescriptions.length > 0;
      });
  }  
}
