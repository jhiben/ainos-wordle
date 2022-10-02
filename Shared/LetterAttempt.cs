namespace BlazorApp.Shared;

public class LetterAttempt
{
    public LetterAttempt()
        : this(' ')
    {
    }

    public LetterAttempt(char letter)
    {
        Letter = letter;
    }

    public char Letter { get; set; }

    public bool IsCorrect { get; set; }

    public bool IsInWrongPlace { get; set; }

    public bool IsUseless => !IsCorrect && !IsInWrongPlace;

    public void Reset()
    {
        Letter = ' ';
    }
}
