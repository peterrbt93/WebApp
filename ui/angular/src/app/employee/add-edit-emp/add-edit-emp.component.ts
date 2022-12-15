import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { SharedService } from 'src/app/shared.service';
import { ShowEmpComponent } from '../show-emp/show-emp.component';

@Component({
  providers: [ShowEmpComponent],
  selector: 'app-add-edit-emp',
  templateUrl: './add-edit-emp.component.html',
  styleUrls: ['./add-edit-emp.component.css']
})

export class AddEditEmpComponent {

  constructor(private service:SharedService, private showEmpComp:ShowEmpComponent) {}

  @Input() emp:any;
  EmployeeId:string = "";
  EmployeeName:string = "";
  Department:string = "";
  DateOfJoining:string = "";
  PhotoFileName:string = "";
  PhotoFilePath:string = "";
  DepartmentsList:any=[];
  @Output() close: EventEmitter<void> = new EventEmitter<void>();

  ngOnInit(): void{
    this.loadDepartmentList();
  }

  loadDepartmentList(){
    this.service.getAllDepartmentNames().subscribe((data:any) => {
      this.DepartmentsList=data;

      this.EmployeeId = this.emp.EmployeeId;
      this.EmployeeName = this.emp.EmployeeName;
      this.Department = this.emp.Department;
      this.DateOfJoining = this.emp.DateOfJoining;
      this.PhotoFileName = this.emp.PhotoFileName;
      this.PhotoFilePath = this.service.PhotoUrl+this.PhotoFileName;
    });
  }

  addEmployee(){
    var val = {EmployeeId: this.EmployeeId,
              EmployeeName: this.EmployeeName,
              Department: this.Department,
              DateOfJoining: this.DateOfJoining,
              PhotoFileName: this.PhotoFileName};
    this.service.addEmployee(val).subscribe(res=>{
      this.close.emit();
    });
  }

  updateEmployee(){
    var val = {EmployeeId: this.EmployeeId,
              EmployeeName: this.EmployeeName,
              Department: this.Department,
              DateOfJoining: this.DateOfJoining,
              PhotoFileName: this.PhotoFileName};
    this.service.updateEmployee(val).subscribe(res=>{
      this.close.emit();
    });
  }

  uploadPhoto(event:Event){
    if (event && event.target){
      var file = (event.target as HTMLInputElement).files![0];
      const formData:FormData = new FormData();
      formData.append('uploadedFile', file, file.name);

      this.service.uploadPhoto(formData).subscribe((data:any)=>{
        // when we submit the photo to the database we save 
        // the changed photo with data we get from the api response (filename)
        this.PhotoFileName = data.toString();
        this.PhotoFilePath = this.service.PhotoUrl+this.PhotoFileName;
      });
    }
  }
}

interface HTMLInputEvent extends Event {
  target: HTMLInputElement & EventTarget;
}
