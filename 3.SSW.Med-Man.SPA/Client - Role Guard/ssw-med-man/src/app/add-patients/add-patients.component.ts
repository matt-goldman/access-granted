import { Component, OnInit } from '@angular/core';
import { PatientDTO, PatientsClient } from '../../helpers/api-client';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-add-patients',
  templateUrl: './add-patients.component.html',
  styleUrls: ['./add-patients.component.css']
})
export class AddPatientsComponent implements OnInit {

  addPatientDTO: PatientDTO = {id: null, familyName:'', givenName: '', dob: null};

  constructor(private patientClient: PatientsClient, private router: Router, private _snakBar: MatSnackBar) { }

  ngOnInit() {
  }

  addPatient({value} : {value : PatientDTO }){
    this.patientClient.postPatient(value)
    .subscribe(
      result => {
        if(result) {
          this._snakBar.open("Patient added!", "OK" , {duration: 3000});
          this.router.navigate(['/patients']);
        } else {
          this._snakBar.open("Adding patient failed", "OK" , {duration: 3000});
        }
      },
      error => {
        this._snakBar.open("Adding patient failed", "OK" , {duration: 3000});
        console.log(error);
      });
  }

}
