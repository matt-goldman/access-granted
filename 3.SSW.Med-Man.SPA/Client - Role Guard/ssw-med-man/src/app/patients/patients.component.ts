import { Component, OnInit } from '@angular/core';
import { PatientDTO, PatientsClient } from '../../helpers/api-client';
import {MatDialog} from '@angular/material/dialog';
import { MedListComponent } from '../med-list/med-list.component';

@Component({
  selector: 'app-patients',
  templateUrl: './patients.component.html',
  styleUrls: ['./patients.component.css']
})
export class PatientsComponent implements OnInit {

  patients: PatientDTO[] = [];

  displayedColumns: string[] = ['name', 'dob', 'scripts', 'admins'];

  arePatients: boolean;

  constructor(private patientClient: PatientsClient) { }

  ngOnInit() {
    this.patientClient.getPatients()
    .subscribe(result => {
      this.patients = result;
      this.arePatients = this.patients.length > 0;
    });
  }

}