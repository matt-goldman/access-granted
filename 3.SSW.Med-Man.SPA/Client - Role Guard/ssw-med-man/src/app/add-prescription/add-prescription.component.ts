import { Component, OnInit } from '@angular/core';
import { PrescriptionDTO, PrescriptionsClient, PatientsClient, MedicationsClient, PatientDTO, MedicationDTO } from '../../helpers/api-client';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';

@Component({
  selector: 'app-add-prescription',
  templateUrl: './add-prescription.component.html',
  styleUrls: ['./add-prescription.component.css']
})
export class AddPrescriptionComponent implements OnInit {

  addScriptDTO: PrescriptionDTO = {id: 0, medication:{}, patient: {}, dose: 0};

  constructor(private scriptClient: PrescriptionsClient, private router: Router, private _snakBar: MatSnackBar, private patientsClient: PatientsClient, private medicationsClient: MedicationsClient) { }

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

  addPrescription(){
    this.addScriptDTO.dose = +this.doseControl.value;
    this.addScriptDTO.medication.id = this.medicationControl.value;
    this.addScriptDTO.patient.id = this.patientControl.value;
    console.log("Adding prescription:");
    console.log(this.addScriptDTO);
    this.scriptClient.postPrescription(this.addScriptDTO)
    .subscribe(
      result => {
        if(result) {
          this._snakBar.open("Prescription added!", "OK" , {duration: 3000});
          this.router.navigate(['/prescriptions']);
        } else {
          this._snakBar.open("Adding prescriptions failed", "OK" , {duration: 3000});
        }
      },
      error => {
        this._snakBar.open("Adding prescription failed", "OK" , {duration: 3000});
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
