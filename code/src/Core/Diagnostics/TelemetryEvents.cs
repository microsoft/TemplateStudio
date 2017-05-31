// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

namespace Microsoft.Templates.Core.Diagnostics
{
    public class TelemetryEvents
    {
        public static string ProjectGen { get; private set; } = TelemetryTracker.PropertiesPrefix + "ProjectGen";
        public static string PageGen { get; private set; } = TelemetryTracker.PropertiesPrefix + "PageGen";
        public static string FeatureGen { get; private set; } = TelemetryTracker.PropertiesPrefix + "FeatureGen";
        public static string Wizard { get; private set; } = TelemetryTracker.PropertiesPrefix + "Wizard";
        public static string SessionStart { get; private set; } = TelemetryTracker.PropertiesPrefix + "SessionStart";
        public static string EditSummaryItem { get; internal set; } = TelemetryTracker.PropertiesPrefix + "EditSummaryItem";
    }
}
