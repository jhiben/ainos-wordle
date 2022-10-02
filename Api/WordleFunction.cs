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
            1 => "excel",
            2 => "crash",
            3 => "state",
            4 => "group",
            5 => "field",
            6 => "turbo",
            7 => "macro",
            8 => "const",
            9 => "debug",
            10 => "layer",
            11 => "idiom",
            12 => "reset",
            13 => "stack",
            14 => "query",
            15 => "queue",
            16 => "virus",
            17 => "input",
            18 => "plsql",
            19 => "cobol",
            20 => "cache",
            21 => "linux",
            22 => "mouse",
            23 => "msdos",
            24 => "patch",
            25 => "proxy",
            26 => "owasp",
            27 => "tcpip",
            28 => "azure",
            29 => "parse",
            30 => "where",
            31 => "monad",
        };
}
