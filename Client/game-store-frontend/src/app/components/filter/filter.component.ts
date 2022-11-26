import {Component, ElementRef, HostListener, OnInit, ViewChild} from '@angular/core';
import {GameService} from "../../services/game.service";

@Component({
  selector: 'app-filter',
  templateUrl: './filter.component.html',
  styleUrls: ['./filter.component.css']
})
export class FilterComponent implements OnInit {
  isOpen = false;
  timeout: ReturnType<typeof setTimeout>;
  @ViewChild("inputElement") inputEl: ElementRef;
  @HostListener('document:click', ['$event']) toggleOpen(event: Event) {
    this.isOpen = !!(this.isOpen && this.elRef.nativeElement.contains(event.target));
  }
  @HostListener('keyup', ['$event']) searchByName(event: Event) {
    clearTimeout(this.timeout);

    this.timeout = setTimeout(() => {
      console.log('Input Value:', this.inputEl.nativeElement.value);
      if (this.inputEl.nativeElement.value.length >= 3) {
        this.gameService.nameFilteringChanged(this.inputEl.nativeElement.value);
      }
      if (this.inputEl.nativeElement.value === "") {
        this.gameService.nameFilteringChanged("");
      }
    }, 1000);
  }
  constructor(private elRef: ElementRef, private gameService: GameService) {}

  ngOnInit(): void {
  }

  onOpenClick(){
    if(!this.isOpen){
      this.isOpen = true;
    }
  }
}
