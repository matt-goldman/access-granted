import { Component, OnInit, Input } from '@angular/core';
import { MedicationDTO } from 'src/helpers/api-client';

@Component({
  selector: 'app-med-list',
  templateUrl: './med-list.component.html',
  styleUrls: ['./med-list.component.css']
})
export class MedListComponent implements OnInit {

  constructor() { }

  @Input()
  displayedColumns: string[];

  medications: MedicationDTO[];

  areMedications: boolean;

  ngOnInit() {
  }

}
