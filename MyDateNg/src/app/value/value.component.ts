import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-value',
  templateUrl: './value.component.html',
  styleUrls: ['./value.component.css']
})
export class ValueComponent implements OnInit {
  baseUrl = environment.apiUrl;
  values: any;

  constructor(private http: HttpClient) { }

  ngOnInit() {
    this.getValues();
  }
getValues(){
  this.http.get('values').subscribe(response => {
    this.values = response;
  }, error => {
    console.log(error);
  });
}

}
