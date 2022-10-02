using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using BlazorApp.Shared;

namespace BlazorApp.Api;

public static class WordleFunction
{
    [FunctionName("wordle")]
    public static IActionResult Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
        ILogger log)
    {
        if (!TryParseQuery(req, out var attempt))
        {
            return new BadRequestResult();
        }

        log.LogInformation("Word {word} submitted", attempt);

        var word = GetTodayWord();

        var selected = attempt.Select(l => new LetterAttempt(l)).ToArray();

        foreach (var (selection, letter) in selected.Zip(word))
        {
            if (selection.Letter == letter)
            {
                selection.IsCorrect = true;
            }

            foreach (var s in selected)
            {
                if (s.Letter == letter)
                {
                    s.IsInWrongPlace = true;
                }
            }
        }

        return new OkObjectResult(new SubmitWordleResult(selected));
    }

    private static bool TryParseQuery(HttpRequest req, out string word)
    {
        word = req.Query["word"];

        return !string.IsNullOrWhiteSpace(word) && word.Length == 5;
    }

    private static string GetTodayWord() =>
        DateTime.Now.Day switch
        {
            1 => "state",
            2 => "group",
            _ => "monad",
        };
}
