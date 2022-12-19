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
      if (index != -1){
        this.DepartmentList.splice(index, 1);
      }
      let indexFilter = this.DepartmentListWithoutFilter.findIndex(x => x.DepartmentId === item.DepartmentId);
      if (indexFilter != -1){
        this.DepartmentListWithoutFilter.splice(indexFilter, 1);
      }
      console.log(index);
      console.log(indexFilter);
    });
  }

  filterFunction(){
    var DepartmentIdFilter = this.DepartmentIdFilter;
    var DepartmentNameFilter = this.DepartmentNameFilter;
    var idFilter = DepartmentIdFilter.toString().toLowerCase();
    var departmentFilterIdList:string[] = [];
    var lower:number;
    var upper:number;

    if (idFilter.charAt(0)=="(") {
      lower = parseInt(idFilter.slice(1,-1).split(":")[0]);
      upper = parseInt(idFilter.slice(1,-1).split(":")[1]);
      for (var i = lower; i <= upper; i++) {
        departmentFilterIdList.push(i.toString());
      }
    } else {
      departmentFilterIdList = [idFilter];
    }
    console.log(departmentFilterIdList);
    
    this.DepartmentList = this.DepartmentListWithoutFilter.filter(function (el){
      return ShowDepComponent.contains(el.DepartmentId.toString().toLowerCase(),
        departmentFilterIdList
      ) && 
      el.DepartmentName.toString().toLowerCase().includes(
        DepartmentNameFilter.toString().trim().toLowerCase()
      )
    });
  }

  
  static contains(target:string, pattern:string[]):boolean{
    console.log(target);
    var value = 0;
    pattern.forEach(function(word){
      value = value + (target==word ? 1 : 0);
      if (word=="") {value=1;}
    });
    return (value === 1)
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
