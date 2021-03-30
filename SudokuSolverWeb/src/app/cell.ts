export class Cell {

    public static readonly EmptyValue: number = 0;
 //   public static EMPTY(): Cell {
 //       return new Cell(0, false, false, false, false, []);
 //   }

    constructor(
        public value: number = 0,
        public excluded: boolean = false,
        public highlight: boolean = false,
        public fiftyFiftyExclusion: boolean = false,
        public guessed: boolean = false,
        public fiftyFifties: Set<number> = new Set<number>()
    ) {}

    public clone(): Cell {
        return new Cell(this.value, this.excluded, this.highlight, this.fiftyFiftyExclusion, this.guessed, this.fiftyFifties);
    }
}
