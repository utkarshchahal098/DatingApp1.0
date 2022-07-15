import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { User } from './_models/user';
import { AccountService } from './_Services/account.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{
  title = 'Dating App';
  users: any;

  constructor(private http: HttpClient, private accountservice: AccountService){

  }

  ngOnInit(){
    this.getUsers(); 
    this.setCurrentUser();
  }
   
  setCurrentUser(){
    const user:User = JSON.parse(localStorage.getItem('user'));
    this.accountservice.setCurrentUser(user);
  }
  getUsers(){
    this.http.get('https://localhost:5001/api/users').subscribe(response => {
      this.users = response;
    },error =>{
       console.log(error)
    });
  }
}
