import {EventEmitter} from "@angular/core";

export interface ISearchingByName {
  selectedNamesChanged: EventEmitter<string>
  nameFilteringChanged(name: string): void;
}
