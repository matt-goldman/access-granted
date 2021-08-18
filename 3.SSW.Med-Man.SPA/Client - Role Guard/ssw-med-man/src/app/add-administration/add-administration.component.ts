import { Component, OnInit } from '@angular/core';
import { AdministrationDTO, AdministrationsClient, PatientsClient, MedicationsClient, PatientDTO, MedicationDTO } from 'src/helpers/api-client';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Observable } from 'rxjs';
import {FormControl, ReactiveFormsModule} from '@angular/forms';
import {map, startWith} from 'rxjs/operators';

@Component({
  selector: 'app-add-administration',
  templateUrl: './add-administration.component.html',
  styleUrls: ['./add-administration.component.css']
})
export class AddAdministrationComponent implements OnInit {

  addAdminDTO: AdministrationDTO = {id: 0, medication:{}, patient: {}, dose: 0};

  constructor(private adminClient: AdministrationsClient, private router: Router, private _snakBar: MatSnackBar, private patientsClient: PatientsClient, private medicationsClient: MedicationsClient) { }

  patients: PatientDTO[] = [];
  filteredPatients: Observable<PatientDTO[]>;

  medications: MedicationDTO[] = [];
  filteredMeds: Observable<MedicationDTO[]>;

  medicationControl = new FormControl;
  patientControl = new FormControl;
  doseControl = new FormControl;

  ngOnInit() {
    this.patientsClient.getPatients()
    .subscribe(result => {
      this.patients = result;
    });

    this.filteredPatients = this.patientControl.valueChanges
      .pipe(
        startWith(''),
        map(value => typeof value === 'string' ? value : value.fullName),
        map(name => name ? this._filterPatients(name) : this.patients.slice())
    );

    this.medicationsClient.getMedications()
    .subscribe(result => {
      this.medications = result;
    });

    this.filteredMeds = this.medicationControl.valueChanges
    .pipe(
      startWith(''),
      map(value => typeof value === 'string' ? value : value.name),
      map(name => name ? this._filterMeds(name) : this.medications.slice())
    );
  }

  displayPatientFn(patient?: PatientDTO): string | undefined {
    return patient ? patient.fullName : undefined;
  }

  displayMedicationFn(medication?: MedicationDTO): string | undefined {
    return medication ? medication.name : undefined;
  }

  addAdministration(){
    this.addAdminDTO.dose = +this.doseControl.value;
    this.addAdminDTO.medication.id = this.medicationControl.value;
    this.addAdminDTO.patient.id = this.patientControl.value;
    this.addAdminDTO.timeGiven = new Date();
    console.log("Administering medication:");
    console.log(this.addAdminDTO);
    this.adminClient.postAdministrations(this.addAdminDTO)
    .subscribe(
      result => {
        if(result) {
          this._snakBar.open("Medication administered!", "OK" , {duration: 3000});
          this.router.navigate(['/prescriptions']);
        } else {
          this._snakBar.open("Adding administration failed", "OK" , {duration: 3000});
        }
      },
      error => {
        this._snakBar.open("Adding administration failed", "OK" , {duration: 3000});
        console.log(error);
      });
  }

  private _filterPatients(value: string): PatientDTO[] {
    const filterValue = value.toLowerCase();

    return this.patients.filter(patient => patient.fullName.toLowerCase().includes(filterValue));
  }

  private _filterMeds(value: string): MedicationDTO[] {
    const filterValue = value.toLowerCase();

    return this.medications.filter(med => med.name.toLowerCase().includes(filterValue));
  }

}