import {Component, ElementRef, HostListener, OnInit} from '@angular/core';
import {GameService} from "../../services/game.service";

@Component({
  selector: 'app-filter',
  templateUrl: './filter.component.html',
  styleUrls: ['./filter.component.css']
})
export class FilterComponent implements OnInit {
  isOpen = false;
  @HostListener('document:click', ['$event']) toggleOpen(event: Event) {
    this.isOpen = !!(this.isOpen && this.elRef.nativeElement.contains(event.target));
  }
  constructor(private elRef: ElementRef, public gameService: GameService) {}

  ngOnInit(): void {
  }

  onOpenClick(){
    if(!this.isOpen){
      this.isOpen = true;
    }
  }
}
