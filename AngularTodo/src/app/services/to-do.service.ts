import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { HttpClient } from '@angular/common/http';  // to hit controller endpoints instead of hub connection
import { BehaviorSubject, Observable } from 'rxjs';
import { Howl } from 'howler';  // 1) $ npm i howler 
//                                 2) $ npm i --save-dev @types/howler

@Injectable({
  providedIn: 'root',
})

export class ToDoService {
  private hubConnection: signalR.HubConnection;
  private toDoListSubject: BehaviorSubject<ToDoItem[]> = new BehaviorSubject<
    ToDoItem[]
  >([]);

  private sound: Howl;  // Sound player

  private apiUrl = 'http://localhost:5001/api/ToDo'; // Adjust the API URL as needed

  constructor(private http: HttpClient) {
    // Initialize the Howl sound
    this.sound = new Howl({
      src: ['assets/booking-request-received.mp3'], // Replace with your audio file path
      volume: 0.9,
    });

    this.hubConnection = new signalR.HubConnectionBuilder()
      // if try to fetch https => ERR SSL
      .withUrl('http://localhost:5001/todoHub') // Adjust the URL as needed
      .build();

    this.startConnection();

    // Subscribe to the 'ReceiveUpdate' event and update the BehaviorSubject
    this.hubConnection.on('ReceiveUpdate', (data) => {
      this.playSound();
      console.log("ReceiveUpdate data: ", data);
      this.toDoListSubject.next(data);
    });
  }

  playSound() {
    // Play the sound
    this.sound.play();
  }

  startConnection = () => {
    this.hubConnection
      .start()
      .then(() => {
        console.log('Connection started');        
      })
      .catch((err) => console.log('Error while starting connection: ' + err));
  };


  getToDoListObservable(): Observable<ToDoItem[]> {
    return this.toDoListSubject.asObservable();
  }

  // getToDoList = (): void => {
  //   this.hubConnection.invoke('GetToDoItems').then((data) => {
  //     console.log('Received ToDo items:', data); // Log the received items
  //     this.toDoListSubject.next(data);
  //   });
  // };
  

  // addToDoItem = (item: ToDoItem): void => {
  //   console.log('Todo to add: ', item);
  //   this.hubConnection.invoke('AddToDoItem', item);
  // };

  // updateToDoItem = (id: string, item: ToDoItem): void => {
  //   this.hubConnection.invoke('UpdateToDoItem', id, item);
  // };

  // deleteToDoItem = (id: string): void => {
  //   this.hubConnection.invoke('DeleteToDoItem', id);
  // };

  getToDoList(): void {
    this.http.get<ToDoItem[]>(this.apiUrl).subscribe((data) => {
      console.log('Received ToDo items:', data);
      this.toDoListSubject.next(data);
    });
  }

  addToDoItem(item: ToDoItem): void {
    console.log('Todo to add: ', item);
    this.http.post(this.apiUrl, item).subscribe(() => {
      this.playSound();
      this.getToDoList(); // Refresh the ToDo list after adding an item
    });
  }

  updateToDoItem(id: string, item: ToDoItem): void {
    const updateUrl = `${this.apiUrl}/${id}`;
    this.http.put(updateUrl, item).subscribe(() => {
      this.getToDoList(); // Refresh the ToDo list after updating an item
    });
  }

  deleteToDoItem(id: string): void {
    const deleteUrl = `${this.apiUrl}/${id}`;
    this.http.delete(deleteUrl).subscribe(() => {
      this.getToDoList(); // Refresh the ToDo list after deleting an item
    });
  }
}

export interface ToDoItem {
  id: string;
  text: string;
  isCompleted: boolean;
}
