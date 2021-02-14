export class SudokuCell {

    constructor(
        public value: number,
        public excluded: boolean,
        public highlight: boolean,
        public fiftyFiftyExclusion: boolean,
        public guessed: boolean,
        public fiftyfifty: number[]
    ) {}
}
