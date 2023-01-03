import {Component, ElementRef, HostListener, Input, OnInit, ViewChild} from '@angular/core';
import {GameService} from "../../../services/game.service";

@Component({
  selector: 'app-filter-name',
  templateUrl: './filter-name.component.html',
  styleUrls: ['./filter-name.component.css']
})
export class FilterNameComponent implements OnInit {
  @Input() serviceWithFiltering: GameService;
  timeout: ReturnType<typeof setTimeout>;
  @ViewChild("inputElement") inputEl: ElementRef;
  @HostListener('keyup', ['$event']) searchByName() {
    clearTimeout(this.timeout);

    this.timeout = setTimeout(() => {
      console.log('Input Value:', this.inputEl.nativeElement.value);
      if (this.inputEl.nativeElement.value.length >= 3) {
        this.serviceWithFiltering.nameFilteringChanged(this.inputEl.nativeElement.value);
      }
      if (this.inputEl.nativeElement.value === "") {
        this.serviceWithFiltering.nameFilteringChanged("");
      }
    }, 1000);
  }
  constructor() { }

  ngOnInit(): void {
  }

}
