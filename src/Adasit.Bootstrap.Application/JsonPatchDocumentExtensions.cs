namespace Adasit.Bootstrap.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using Adasit.Bootstrap.Application.Dto;
using Adasit.Bootstrap.Application.Dto.Models.Errors;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;

public static class JsonPatchDocumentExtensions
{
    public static void Validate<T>(
        this JsonPatchDocument<T> payload,
        OperationType acceptedOperation,
        List<string> acceptedPaths) where T : class
    {
        var operations = payload.Operations.Where(x => x.OperationType == acceptedOperation);

        if (!operations.Any())
        {
            var err = ErrorCodeConstant.InvalidOperationOnPatch();

            var collection = payload.Operations.Select(x => x.OperationType).ToList();

            var op = "";

            foreach (var item in collection)
            {
                op += $"{item},";
            }

            err.ChangeInnerMessage(op ?? "");

            throw new BusinessException(err);
        }

        if (operations.Any(x => !acceptedPaths.Contains(x.path, StringComparer.OrdinalIgnoreCase)))
        {
            var err = ErrorCodeConstant.InvalidPathOnPatch();

            var collection = payload.Operations
                .Where(x => !acceptedPaths.Contains(x.path, StringComparer.OrdinalIgnoreCase))
                .Select(x => x.path)
                .ToList();

            var op = "";

            foreach (var item in collection)
            {
                op += $"{item},";
            }

            err.ChangeInnerMessage(op ?? "");

            throw new BusinessException(err);
        };
    }
}