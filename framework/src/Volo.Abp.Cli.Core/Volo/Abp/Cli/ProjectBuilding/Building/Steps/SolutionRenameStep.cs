﻿using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp.Cli.ProjectBuilding.Files;

namespace Volo.Abp.Cli.ProjectBuilding.Building.Steps
{
    public class SolutionRenameStep : ProjectBuildPipelineStep
    {
        public override void Execute(ProjectBuildContext context)
        {
            new SolutionRenamer(
                context.Files,
                "MyCompanyName",
                "MyProjectName",
                context.BuildArgs.SolutionName.CompanyName,
                context.BuildArgs.SolutionName.ProjectName
            ).Run();
        }

        private class SolutionRenamer
        {
            private readonly List<FileEntry> _entries;
            private readonly string _companyNamePlaceHolder;
            private readonly string _projectNamePlaceHolder;

            private readonly string _companyName;
            private readonly string _projectName;

            public SolutionRenamer(List<FileEntry> entries, string companyNamePlaceHolder, string projectNamePlaceHolder, string companyName, string projectName)
            {
                if (string.IsNullOrWhiteSpace(companyName))
                {
                    companyName = null;
                }

                if (companyNamePlaceHolder == null && companyName != null)
                {
                    throw new UserFriendlyException($"Can not set {nameof(companyName)} if {nameof(companyNamePlaceHolder)} is null.");
                }

                _entries = entries;

                _companyNamePlaceHolder = companyNamePlaceHolder;
                _projectNamePlaceHolder = projectNamePlaceHolder ?? throw new ArgumentNullException(nameof(projectNamePlaceHolder));

                _companyName = companyName;
                _projectName = projectName ?? throw new ArgumentNullException(nameof(projectName));
            }

            public void Run()
            {
                if (_companyNamePlaceHolder != null && _companyName != null)
                {
                    RenameHelper.RenameAll(_entries, _companyNamePlaceHolder, _companyName);
                }
                else if (_companyNamePlaceHolder != null)
                {
                    RenameHelper.RenameAll(_entries, _companyNamePlaceHolder + "." + _projectNamePlaceHolder, _projectNamePlaceHolder);
                }

                RenameHelper.RenameAll(_entries, _projectNamePlaceHolder, _projectName);
            }
        }
    }
}