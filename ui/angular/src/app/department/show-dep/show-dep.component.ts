import { ChangeDetectorRef, Component } from '@angular/core';
import { SharedService } from 'src/app/shared.service';

@Component({
  selector: 'app-show-dep',
  templateUrl: './show-dep.component.html',
  styleUrls: ['./show-dep.component.css']
})
export class ShowDepComponent {

  constructor(private service:SharedService, private cd:ChangeDetectorRef,
    ) {}

  DepartmentList:any[]= [];
  ModalTitle:string ="";
  ActivateAddEditDepComp:boolean=false;
  dep:any;


  ngOnInit(): void {
    this.refreshDepList();
  }

  addClick(){
    this.dep={
      DepartmentId:0,
      DepartmentName:""
    }
    this.ModalTitle = "Add department";
    this.ActivateAddEditDepComp = true;
  }

  editClick(item:any){
    this.dep=item;
    this.ModalTitle = "Edit department";
    this.ActivateAddEditDepComp = true;
  }

  closeClick(){
    this.ActivateAddEditDepComp = false;
    this.refreshDepList();
  }

  refreshDepList(){
    this.service.getDepList().subscribe(data=>{
      this.DepartmentList=data;
      this.cd.detectChanges();
    });
  }

  deleteClick(item:any){
    let index = this.DepartmentList.findIndex(x => x.DepartmentId === item.DepartmentId);
    this.service.deleteDepartment(item.DepartmentId).subscribe(data=>{
      this.DepartmentList.splice(index, 1);
    });
  }

}
