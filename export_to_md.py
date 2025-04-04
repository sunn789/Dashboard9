import os
from pathlib import Path

EXCLUDED_DIRS = {'.git', 'vendor', 'node_modules', '__pycache__', 'bin'}  # Directories to skip
EXCLUDED_EXT = {'.exe', '.dll', '.png', '.jpg', '.pdf', '.zip'}  # Binary files to skip

def export_to_md(root_dir, output_file="project_analysis.md"):
    with open(output_file, 'w', encoding='utf-8') as md:
        md.write("# Go Project Analysis\n\n")
        for root, dirs, files in os.walk(root_dir):
            dirs[:] = [d for d in dirs if d not in EXCLUDED_DIRS]
            for file in files:
                filepath = Path(root) / file
                # Skip files with the .env extension or containing '.env' in the name
                if filepath.suffix == '.env' or '.env' in filepath.name:
                    continue
                if filepath.suffix in EXCLUDED_EXT:
                    continue  # Skip binary files
                try:
                    content = filepath.read_text(encoding='utf-8')
                except UnicodeDecodeError:
                    continue  # Skip files that can't be read as text
                rel_path = filepath.relative_to(root_dir)
                md.write(f"## File: `{rel_path}`\n")
                md.write(f"```go\n")  # Force syntax highlighting for Go
                md.write(content + "\n")
                md.write("```\n\n")

if __name__ == "__main__":
    export_to_md('.')  # Start from current directory
