import os
import re
from pathlib import Path

EXCLUDED_DIRS = {
    '.git', 'vendor', 'node_modules', '__pycache__', 
    'bin', 'obj', 'wwwroot', 'lib', 'assets', 'packages',
    '.vs', 'TestResults'  # Added common .NET directories
}

EXCLUDED_EXT = {
    '.exe', '.dll', '.png', '.jpg', '.pdf', '.zip',
    '.css', '.scss', '.map', '.ico', '.svg'  # Added web asset extensions
}

SYNTAX_MAP = {
    '.cs': 'csharp',
    '.csproj': 'xml',
    '.json': 'json',
    '.js': 'javascript',
    '.html': 'html'
}

def export_to_md(root_dir, output_file="project_analysis.md"):
    with open(output_file, 'w', encoding='utf-8') as md:
        md.write("# .NET Project Analysis\n\n")
        for root, dirs, files in os.walk(root_dir):
            dirs[:] = [d for d in dirs if d not in EXCLUDED_DIRS]
            for file in files:
                filepath = Path(root) / file
                
                # Skip excluded files and directories
                if (filepath.suffix in EXCLUDED_EXT or 
                    any(part in EXCLUDED_DIRS for part in filepath.parts)):
                    continue
                
                # Skip .env files
                if '.env' in filepath.name.lower():
                    continue

                try:
                    content = filepath.read_text(encoding='utf-8')
                    # Redact connection strings
                    content = re.sub(
                        r'("ApplicationDbContextConnection":\s*)"[^"]*"',
                        r'\1""',
                        content
                    )
                except UnicodeDecodeError:
                    continue

                rel_path = filepath.relative_to(root_dir)
                lang = SYNTAX_MAP.get(filepath.suffix, 'text')
                
                md.write(f"## File: `{rel_path}`\n")
                md.write(f"```{lang}\n{content}\n```\n\n")

if __name__ == "__main__":
    export_to_md('.')