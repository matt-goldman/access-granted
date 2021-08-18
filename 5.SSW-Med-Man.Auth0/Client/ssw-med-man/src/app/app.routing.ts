import {Routes, RouterModule} from '@angular/router';
import { AdministrationsComponent } from './administrations/administrations.component';
import { MedicationsComponent } from './medications/medications.component';
import { PrescriptionsComponent } from './prescriptions/prescriptions.component';
import { PatientsComponent } from './patients/patients.component';
import { HomeComponent } from './home/home.component';
import { AddPatientsComponent } from './add-patients/add-patients.component';
import { AddPrescriptionComponent } from './add-prescription/add-prescription.component';
import { AddAdministrationComponent } from './add-administration/add-administration.component';
import { ModuleWithProviders } from '@angular/core';
import { RoleGuardService } from './role-guard.service';
import { UnauthComponent } from './unauth/unauth.component';

const appRoutes: Routes =[
    {path: 'patients', component:PatientsComponent},
    {path: 'addPatient', component:AddPatientsComponent},
    {path: 'medications', component: MedicationsComponent},
    {path: 'prescriptions', component: PrescriptionsComponent},
    {path: 'addPrescription', component: AddPrescriptionComponent, canActivate: [RoleGuardService], data: {expectedRole: "Doctor"}},
    {path: 'administrations', component: AdministrationsComponent},
    {path: 'addAdministration', component: AddAdministrationComponent, canActivate: [RoleGuardService], data: {expectedRole: "Nurse"}},
    {path: 'home', component:HomeComponent},
    {path: 'unauth', component: UnauthComponent}
];

export const routing: ModuleWithProviders = RouterModule.forRoot(appRoutes);