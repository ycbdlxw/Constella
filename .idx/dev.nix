# To learn more about how to use Nix to configure your environment
# see: https://developers.google.com/idx/guides/customize-idx-env
{ pkgs, ... }: {
  # Which nixpkgs channel to use.
  channel = "stable-24.11"; # or "unstable"
  # Use https://search.nixos.org/packages to find packages
  packages = [ pkgs.dotnet-sdk_9 pkgs.nodejs_20 ];
  # Sets environment variables in the workspace
  env = { };
  idx = {
    # Search for the extensions you want on https://open-vsx.org/ and use "publisher.id"
    extensions = [ "muhammad-sammy.csharp" "rangav.vscode-thunder-client" ];
    workspace = {
      # Runs when a workspace is (re)started
      onStart = { run-server = "dotnet watch --urls=http://0.0.0.0:8081"; };
    };
    previews = [
      {
        # The command to run to start the server
        command = ["dotnet", "watch", "--urls=http://0.0.0.0:8081"];
        # The port the server will be listening on
        port = 8081;
        # The label to display in the web preview
        label = "backend";
      }
    ];
  };
}
