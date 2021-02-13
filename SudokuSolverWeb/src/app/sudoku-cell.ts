export class SudokuCell {
    value: number
    excluded: boolean
    highlight: boolean
    fiftyFiftyExclusion: boolean
    guessed: boolean

    fiftyfifty: number[]
}
