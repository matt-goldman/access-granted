import { Component, OnInit } from '@angular/core';
import { AdministrationsClient, AdministrationDTO } from '../../helpers/api-client';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-administrations',
  templateUrl: './administrations.component.html',
  styleUrls: ['./administrations.component.css']
})
export class AdministrationsComponent implements OnInit {

  constructor(private adminClient: AdministrationsClient, private dialog: MatDialog, private snackBar: MatSnackBar) { }

  displayedColumns: string[] = ['patientName', 'medication','dose', 'timeGiven'];

  administrations: AdministrationDTO[] = [];

  areAdmins: boolean;

  ngOnInit() {
    this.adminClient.getAdministrationsAll()
    .subscribe(
      (result) => {
        this.administrations = result;
        this.areAdmins = this.administrations.length > 0;
      });
  }  
}
