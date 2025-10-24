import { PageInfo } from "../models/PageInfo";
import { Post } from "../models/Post";


/**
 * Page view dto.
 */
export class PageViewDto {
  /**
   * Page view dto Posts list.
   * @param posts array
   * Page view dto Display type.
   * @param displayType string
   * Page view dto Page info.
   * @param pageInfo PageInfo
   */
  constructor(
    public posts: Post[],
    public displayType: string,
    public pageInfo: PageInfo
  ) {}
}
