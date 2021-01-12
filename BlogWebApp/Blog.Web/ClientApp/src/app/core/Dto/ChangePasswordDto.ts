/**
 * Change password dto.
 */
export class ChangePasswordDto {
  /**
   * @param oldPassword string
   * @param newPassword string
   */
  constructor(
    public oldPassword: string,
    public newPassword: string,
  ) {}
}
