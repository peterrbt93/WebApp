import { ChangeDetectorRef, Component } from '@angular/core';
import { SharedService } from 'src/app/shared.service';

@Component({
  selector: 'app-show-emp',
  templateUrl: './show-emp.component.html',
  styleUrls: ['./show-emp.component.css']
})
export class ShowEmpComponent {

  constructor(private service:SharedService, private cd:ChangeDetectorRef,
    ) {}

  EmployeeList:any[]= [];
  ModalTitle:string ="";
  ActivateAddEditEmpComp:boolean=false;
  emp:any;


  ngOnInit(): void {
    this.refreshEmpList();
  }

  addClick(){
    this.emp={
      EmployeeId:0,
      EmployeeName:"",
      Department:"",
      DateOfJoining:"",
      PhotoFileName:"anonymous.png"
    }
    this.ModalTitle = "Add employee";
    this.ActivateAddEditEmpComp = true;
  }

  editClick(item:any){
    this.emp=item;
    this.ModalTitle = "Edit employee";
    this.ActivateAddEditEmpComp = true;
  }

  closeClick(){
    this.ActivateAddEditEmpComp = false;
    this.refreshEmpList();
  }

  refreshEmpList(){
    this.service.getEmpList().subscribe(data=>{
      this.EmployeeList=data;
      this.cd.detectChanges();
    });
  }

  deleteClick(item:any){
    let index = this.EmployeeList.findIndex(x => x.EmployeeId === item.EmployeeId);
    this.service.deleteEmployee(item.EmployeeId).subscribe(data=>{
      this.EmployeeList.splice(index, 1);
    });
  }
}
