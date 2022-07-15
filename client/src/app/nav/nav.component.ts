import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable, observable } from 'rxjs';
import { User } from '../_models/user';
import { AccountService } from '../_Services/account.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

  model: any = {};
  currentUser$:Observable<User> ;
  constructor(public accountservice:AccountService, private router: Router,
     private toastr: ToastrService) { }
  

  ngOnInit(): void {
    this.currentUser$ = this.accountservice.currentUser$;
    console.log(this.currentUser$);
  }

  login(){
    this.accountservice.login(this.model).subscribe(response =>
      {this.router.navigateByUrl('/members');
      },error =>{
        console.log(error);
        this.toastr.error(error.error);
      });
      
  }

  logout(){
    this.accountservice.logout();
    this.router.navigateByUrl('/');
  }

  
}
