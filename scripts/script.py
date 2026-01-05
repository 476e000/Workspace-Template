from data_types.workspace_template import Variant, Workspace
from pathlib import Path
from dataclasses import dataclass

Workspace.TEMPLATE_ROOT = Path(__file__).parent.parent

workspaces: dict[str, Workspace] = {}

for dir in Workspace.TEMPLATE_ROOT.iterdir():
	if not dir.is_dir():
		continue
	if dir.match("config.*"):
		name = dir.name.split(".")[1]
		# make_template(configs, name, dir.absolute())
	elif dir.match("*-workspace"):
		name = dir.name.split("-")[0]
		workspaces[name] = Workspace(dir.absolute())


print(workspaces)


def parse_args():
    from argparse import ArgumentParser

    parser = ArgumentParser(description="Create a new project from a template")
    required_group = parser.add_mutually_exclusive_group(required=True)

    required_group.add_argument(
        "name", type=str,
        nargs="?",
        help="the name of the project"
    )
    required_group.add_argument(
        "-l", "--list", type=str,
        nargs="?",
        const="all", default=None,
        help="list available templates and configs",
        metavar="NAME"
    )

    parser.add_argument(
        "-p", "--path", type=Path,
        default=Path.cwd(),
        help="the path to create the project in, if path does not exists it will be created"
    )
    parser.add_argument(
        "-t", "--template", type=str,
        help="the template to create, the format is name:variant, if variant is omitted, default is used"
    )
    parser.add_argument(
        "-c", "--config", type=str,
        help="the config to use"
    )
    parser.add_argument(
        "--always-nest", action="store_true",
        help="create project in subfolder even if project name matches path name"
    )

    @dataclass
    class Args:
        always_nest: bool
        template: str
        variant: str | None
        config: str | None

        path: Path
        name: str

    args = parser.parse_args()

    if args.list:
        # print_info(args.list)
        return

    template_info: list[str] = args.template.split(":")

    return Args(
        always_nest=args.always_nest,
        template=template_info[0],
        variant=template_info[1] if len(template_info) > 1 else None,
        config=args.config,
        path = args.path,
        name = args.name
    )

args = parse_args()
if not args:
    exit(0)

print(args)
# if args.list:
#     print_info(args.list)
#     exit(0)
#
# always_nest: bool = args.always_nest
# template: str = args.template
# variant: str | None = None
#
# config: str = args.config
# path: Path = Path(args.path).absolute()
# name: str = args.name

