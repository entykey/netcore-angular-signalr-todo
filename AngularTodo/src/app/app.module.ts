import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule } from '@angular/forms'; // Add this line
import { HttpClientModule } from '@angular/common/http';


import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ToDoComponent } from './to-do/to-do.component';

@NgModule({
  declarations: [AppComponent, ToDoComponent],
  imports: [
    BrowserModule, 
    AppRoutingModule, 
    FormsModule, // Include FormsModule here
    ReactiveFormsModule, // Include ReactiveFormsModule here
    HttpClientModule  // for http api call
  ],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule {}