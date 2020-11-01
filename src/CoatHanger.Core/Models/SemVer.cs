using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Schema;

namespace CoatHanger.Core.Models
{
    public readonly struct SemVer
    {
        private readonly int _major;
        private readonly int _minor;
        private readonly int _patch;
        private readonly int? _alpha;
        private readonly int? _beta;

        /// <summary>
        /// Creates a Semantic Versioning
        /// </summary>
        /// <param name="major">MAJOR version when you make incompatible API changes.</param>
        /// <param name="minor">MINOR version when you add functionality in a backwards compatible manner.</param>
        /// <param name="patch">PATCH version when you make backwards compatible bug fixes.</param>
        /// <param name="alpha">ALPHA version when you MAY want to A pre-release version</param>
        /// <param name="beta">ALPHA version when you MAY want to A release-candidate version</param>
        public SemVer(int major, int minor, int patch, int? alpha = null, int? beta = null)
        {
            _major = major;
            _minor = minor;
            _patch = patch;
            _alpha = alpha;
            _beta = beta;
        }

        /// <summary>
        /// Converts the Semantic Version into a numeric value used the can be used for sorting.
        /// </summary>
        public long Sequence()
        {
            long majorRank = _major * 100000000;
            long minorRank = _minor * 1000000;
            long patchRank = _patch * 10000;
            long betaRank = (_beta ?? 0) * 100;
            long alphaRank = (_alpha ?? 0);
            
            return majorRank + minorRank + patchRank + alphaRank + betaRank;
        }

        public override string ToString()
        {
            var semVer = $"{_major}.{_minor}.{_patch}";

            if (_alpha.HasValue)
            {
                semVer += $"-alpha.{_alpha}";
            }

            if (_beta.HasValue)
            {
                semVer += $"-beta.{_beta}";
            }

            return semVer;
        }
    }
}
