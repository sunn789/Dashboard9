import os
import shutil
import re

def copy_and_rename_files(source, destination, old_name, new_name):
    # Ensure the destination directory exists
    if not os.path.exists(destination):
        os.makedirs(destination)

    # Walk through the source directory and copy files
    for root, dirs, files in os.walk(source):
        # Exclude .git, bin, and obj folders
        dirs[:] = [d for d in dirs if d not in ['.git', 'bin', 'obj']]

        # Get the relative path of the current directory
        relative_path = os.path.relpath(root, source)
        target_dir = os.path.join(destination, relative_path)

        # Create corresponding directory in the destination
        if not os.path.exists(target_dir):
            os.makedirs(target_dir)

        # Copy all files from source to destination
        for file_name in files:
            source_file = os.path.join(root, file_name)
            target_file = os.path.join(target_dir, file_name)

            # Copy the file to the new destination
            shutil.copy2(source_file, target_file)

            # Attempt to replace the old name with the new name in the text files
            try:
                with open(target_file, 'r', encoding='utf-8') as file:
                    content = file.read()

                # Replace old project name with new project name
                content = content.replace(old_name, new_name)

                with open(target_file, 'w', encoding='utf-8') as file:
                    file.write(content)

                print(f"Updated file: {target_file}")

            except UnicodeDecodeError:
                # If the file is not a text file, skip replacing content
                print(f"Skipping binary or non-UTF-8 file: {target_file}")

def rename_folders_and_files(destination, old_name, new_name):
    # Walk through the directory again to rename any folder or file that contains the old name
    for root, dirs, files in os.walk(destination, topdown=False):
        for name in dirs:
            if old_name.lower() in name.lower():
                old_folder_path = os.path.join(root, name)
                new_folder_name = name.replace(old_name, new_name)
                new_folder_path = os.path.join(root, new_folder_name)
                os.rename(old_folder_path, new_folder_path)
                print(f"Renamed folder: {old_folder_path} to {new_folder_path}")

        for name in files:
            if old_name.lower() in name.lower():
                old_file_path = os.path.join(root, name)
                new_file_name = name.replace(old_name, new_name)
                new_file_path = os.path.join(root, new_file_name)
                os.rename(old_file_path, new_file_path)
                print(f"Renamed file: {old_file_path} to {new_file_path}")

def main():
    # Ask for user input
    source = input("Enter source project path: ").strip()
    old_name = input("Enter old project name (e.g., Modicom): ").strip()
    new_name = input("Enter new project name (e.g., Dev): ").strip()

    # If the source is '~', set it to the current working directory
    if source == "~":
        source = os.getcwd()

    # Ensure source exists
    if not os.path.exists(source):
        print("Error: Source directory does not exist.")
        return

    # Set the destination path (new project folder with new name)
    destination = os.path.join(os.path.dirname(source), new_name)

    # Check if the destination already exists
    if os.path.exists(destination):
        print("Error: Destination directory already exists.")
        return

    print(f"Creating new project at: {destination}")

    # Step 1: Copy files and replace project name in files
    copy_and_rename_files(source, destination, old_name, new_name)

    # Step 2: Rename folders and files that match the old project name
    rename_folders_and_files(destination, old_name, new_name)

    print(f"Project cloned successfully and renamed to: {new_name}")

if __name__ == "__main__":
    main()
