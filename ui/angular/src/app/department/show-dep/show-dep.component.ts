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

  DepartmentIdFilter:string = "";
  DepartmentNameFilter:string = "";
  DepartmentListWithoutFilter:any[]= [];

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
      this.DepartmentListWithoutFilter=data;
      this.cd.detectChanges();
    });
  }

  deleteClick(item:any){
    
    this.service.deleteDepartment(item.DepartmentId).subscribe(data=>{
      let index = this.DepartmentList.findIndex(x => x.DepartmentId === item.DepartmentId);
      this.DepartmentList.splice(index, 1);
      let indexFilter = this.DepartmentListWithoutFilter.findIndex(x => x.DepartmentId === item.DepartmentId);
      this.DepartmentListWithoutFilter.splice(indexFilter, 1);
      console.log(index);
      console.log(indexFilter);
    });
  }

  filterFunction(){
    var DepartmentIdFilter = this.DepartmentIdFilter;
    var DepartmentNameFilter = this.DepartmentNameFilter;

    this.DepartmentList = this.DepartmentListWithoutFilter.filter(function (el){
      return el.DepartmentId.toString().toLowerCase().includes(
        DepartmentIdFilter.toString().trim().toLowerCase()
      ) && 
      el.DepartmentName.toString().toLowerCase().includes(
        DepartmentNameFilter.toString().trim().toLowerCase()
      )
    });
  }

  sortResult(prop:string, asc:boolean){
    this.DepartmentList = this.DepartmentList.sort(function(a,b){
      if (asc){
        return (a[prop]>b[prop])?1 : ((a[prop]<b[prop])?-1 :0);
      } else {
        return (b[prop]>a[prop])?1 : ((b[prop]<a[prop])?-1 :0);
      }
    });
  }

}
