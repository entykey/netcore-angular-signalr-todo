import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ToDoService {
  private hubConnection: signalR.HubConnection;
  private toDoListSubject: BehaviorSubject<ToDoItem[]> = new BehaviorSubject<
    ToDoItem[]
  >([]);

  constructor() {
    this.hubConnection = new signalR.HubConnectionBuilder()
      // if try to fetch https => ERR SSL
      .withUrl('http://localhost:5001/todoHub') // Adjust the URL as needed
      .build();

    this.startConnection();

    // Subscribe to the 'ReceiveUpdate' event and update the BehaviorSubject
    this.hubConnection.on('ReceiveUpdate', (data) => {
      this.toDoListSubject.next(data);
    });
  }

  startConnection = () => {
    this.hubConnection
      .start()
      .then(() => {
        console.log('Connection started');
        this.getToDoList(); // Fetch initial data after connection is established
      })
      .catch((err) => console.log('Error while starting connection: ' + err));
  };

  getToDoListObservable(): Observable<ToDoItem[]> {
    return this.toDoListSubject.asObservable();
  }

  getToDoList = (): void => {
    this.hubConnection.invoke('GetToDoItems').then((data) => {
      console.log('Received ToDo items:', data); // Log the received items
      this.toDoListSubject.next(data);
    });
  };
  

  addToDoItem = (item: ToDoItem): void => {
    console.log('Todo to add: ', item);
    this.hubConnection.invoke('AddToDoItem', item);
  };

  updateToDoItem = (id: string, item: ToDoItem): void => {
    this.hubConnection.invoke('UpdateToDoItem', id, item);
  };

  deleteToDoItem = (id: string): void => {
    this.hubConnection.invoke('DeleteToDoItem', id);
  };
}

export interface ToDoItem {
  id: string;
  text: string;
  isCompleted: boolean;
}
