import { Directive, ElementRef, HostListener, Input } from '@angular/core';

@Directive({
  selector: '[pkmnBorderCard]',
  standalone: true
})
export class BorderCardDirective {

  private initialColor: string ='#f5f5f5';
  private defaultColor: string = '#009688';
  private defaultHeight: number = 200;

  constructor(private el: ElementRef) { 
    this.setHeight(this.defaultHeight);
    this.setBorder(this.initialColor);
  }

  @Input('pkmnBorderCard') borderColor: string =''; // alias
  
  @HostListener('mouseenter') onMouseEnter(){
    this.setBorder(this.borderColor || this.defaultColor);
  }

  @HostListener('mouseleave') onMouseLeave(){
    this.setBorder(this.initialColor);
  }

  setHeight(height: number){
    this.el.nativeElement.height = `${height}px`;  
  }

  setBorder(color: string){    
    this.el.nativeElement.style.border = `soldi 4x ${color}`;
  }
}