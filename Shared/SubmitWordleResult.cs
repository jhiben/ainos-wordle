using System;
using System.Collections.Generic;

namespace BlazorApp.Shared;

public class SubmitWordleResult
{
    public SubmitWordleResult(IEnumerable<LetterAttempt> attempts)
    {
        Attempts = attempts ?? throw new ArgumentNullException(nameof(attempts));
    }

    public IEnumerable<LetterAttempt> Attempts { get; }
}
