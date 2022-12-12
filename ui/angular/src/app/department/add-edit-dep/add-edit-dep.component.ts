import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { SharedService } from 'src/app/shared.service';
import { ShowDepComponent } from '../show-dep/show-dep.component';

@Component({
  providers: [ShowDepComponent],
  selector: 'app-add-edit-dep',
  templateUrl: './add-edit-dep.component.html',
  styleUrls: ['./add-edit-dep.component.css']
})
export class AddEditDepComponent {

  constructor(private service:SharedService, private showDepComp:ShowDepComponent) {}

  @Input() dep:any;
  DepartmentId:string = "";
  DepartmentName:string = "";
  @Output() close: EventEmitter<void> = new EventEmitter<void>();

  ngOnInit(): void{
    this.DepartmentId = this.dep.DepartmentId;
    this.DepartmentName = this.dep.DepartmentName;
  }

  addDepartment(){
    var val = {DepartmentId: this.DepartmentId,
                DepartmentName: this.DepartmentName};
    this.service.addDepartment(val).subscribe(res=>{
      this.close.emit();
    });
  }

  updateDepartment(){
    var val = {DepartmentId: this.DepartmentId,
                DepartmentName: this.DepartmentName};
    this.service.updateDepartment(val).subscribe(res=>{
      this.close.emit();
    });
  }

}
